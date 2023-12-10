using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Models;
using Weblab.Modules.DB;
using Weblab.Modules.DB.DataModel;

namespace Weblab.Modules.Services;

public class DbManagerService : IDbManager
{
    private readonly ApplicationContext _context;
    private readonly IHash _hashService;
    private readonly IImageService _imageService;
    private DateTime CurrentDateTime => DateTime.UtcNow;
    private async Task<(bool Success, Exception? Exception)> ExecuteInTransaction(Func<Task> func)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await func.Invoke();
            await transaction.CommitAsync();
            return (true, null!);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return (false, e);
        }
    }
    public DbManagerService(ApplicationContext context, IHash hashService, IImageService imageService)
    {
        _context = context;
        _hashService = hashService;
        _imageService = imageService;
    }

    public (GetHomeHtmlStatus Status, string? Result) GetHomeHtml()
    {
        var html = _context.MainPartialViews.Where(entity => entity.Id == MainPartialViewCode.Home)
            .Select(entity => entity.Html)
            .FirstOrDefault();
        if(html != null)
            return (GetHomeHtmlStatus.Success, html);
        else
            return (GetHomeHtmlStatus.UnknownError, null);
    }

    public async Task<RegisterStatus> AddUser(RegisterModel model)
    {
        var user = await _context.UserIdentities.FindAsync(model.Login);
        if(user != null)
            return RegisterStatus.UserExists;
        var context = new ValidationContext(model, null, null);
        var results = new List<ValidationResult>();
        if(Validator.TryValidateObject(model, context, results, true))
        {
            var result = await ExecuteInTransaction(async () =>
            {
                    var passwordHash = _hashService.GeneratePasswordHash(model.Password);
                    var identity = new UserIdentityInfo
                    {
                        Login = model.Login,
                        PasswordHash = passwordHash.Hash,
                        Salt = passwordHash.Salt,
                        Role = Role.User,
                        Email = model.Email
                    };
                    await _context.UserIdentities.AddAsync(identity);
                    await _context.SaveChangesAsync();
            });
            if(result.Success)
            {
                return RegisterStatus.Success;
            }
            else
            {
                return RegisterStatus.UnknownError;
            }
        }
        else return RegisterStatus.InvalidModel;
    }

    public (GetShowStatus Status, ShowModel? Show) GetShow(Guid id)
    {
        try
        {
            var show = _context.Shows.Find(id);
            if(show != null)
            {
                var model = new ShowModel
                {
                    Id = show.Id,
                    Name = show.Name,
                    Description = show.Description,
                    Date = show.Date,
                    LabelImage = show.LabelImage
                };
                return (GetShowStatus.Success, model);
            }
            else
            {
                return (GetShowStatus.NotFound, null);
            }
        }
        catch
        {
            return (GetShowStatus.UnknownError, null);
        }
    }

    public (Status Status, ShortShowModel[]? Shows) GetAllShows()
    {
        try
        {
            var shows = _context.Shows
                .Select(x => new ShortShowModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArray();
            return (Status.Success, shows);
        }
        catch
        {
            return (Status.UnknownError, null);
        }
    }

    public async Task<(GetUserStatus Status, UserIdentityModel? User)> GetUser(string login)
    {
        var userEntry = await _context.UserIdentities
            .FindAsync(login);
        if(userEntry == null)
            return (GetUserStatus.NotFound, null);
        var result = new UserIdentityModel
        {
            Login = userEntry.Login,
            PasswordHash = userEntry.PasswordHash,
            Salt = userEntry.Salt,
            Role = userEntry.Role,
            Email = userEntry.Email
        };
        return (GetUserStatus.Success, result);
    }
    public async Task<bool> AddFeedback(FeedbackModel model, string login)
    {
        var result = await ExecuteInTransaction(async () => {
            var feedback = new Feedback
            {
                UserId = login,
                Label = model.Label,
                Text = model.Text
            };
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        });
        return result.Success;
    }
    public List<ShowModel> GetFavoriteShows(string login)
    {
        return _context.FavoriteShows.AsNoTracking()
            .Where(x => x.UserLogin == login)
            .Select(x => x.Show)
            .Select(x => new ShowModel{
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                LabelImage = x.LabelImage,
                Date = x.Date
            })
            .ToList();
    }
    public async Task<(bool Success, List<ShowModel>? Favorites)> AddFavorite(string login, Guid showId)
    {
        var favoriteShows = GetFavoriteShows(login);
        var favoriteShow = await _context.Shows.FindAsync(showId);
        bool isContain = favoriteShows.Select(x => x.Id).Contains(showId);
        if(isContain)
        {
            return (true, favoriteShows);
        }
        else if(favoriteShow == null)
        {
            return (false, null);
        }
        else
        {
            var addResult = await ExecuteInTransaction(async () => {
                var favorite = new FavoriteShow{
                    UserLogin = login,
                    ShowId = favoriteShow.Id
                };
                await _context.FavoriteShows.AddAsync(favorite);
                await _context.SaveChangesAsync();
            });
            if(addResult.Success)
            {
                var convertedFavoriteShow = new ShowModel{
                    Id = favoriteShow.Id,
                    Name = favoriteShow.Name,
                    Description = favoriteShow.Description,
                    LabelImage = favoriteShow.LabelImage,
                    Date = favoriteShow.Date
                };
                favoriteShows.Add(convertedFavoriteShow);
                return (true, favoriteShows);
            }
            else
            {
                return (false, null);
            }
        }
    }
    public async Task<(bool Success, List<ShowModel>? Favorites)> DeleteFavorite(string login, Guid showId)
    {
        var favoriteShow = await _context.Shows.FindAsync(showId);
        if(favoriteShow == null)
            return (false, null);
        var favoriteShows = GetFavoriteShows(login);
        bool isContain = favoriteShows.Select(x => x.Id).Contains(showId);
        if(!isContain)
        {
            return (true, favoriteShows);
        }
        else
        {
            var deleteResult = await ExecuteInTransaction(async () => {
                var removalShow = _context.FavoriteShows.Where(x => x.UserLogin == login && x.ShowId == showId).First();
                _context.FavoriteShows.Remove(removalShow!);
                await _context.SaveChangesAsync();
            });
            favoriteShows = favoriteShows.Where(x => x.Id != showId).ToList();
            if(deleteResult.Success)
                return (true, favoriteShows);
            else
                return (false, null);
        }
    }
    public async Task<Role?> GetRole(string login)
    {
        var user = await _context.UserIdentities.FindAsync(login);
        if(user != null)
            return user.Role;
        else
            return null;
    }
    public async Task<(bool Success, Guid? ShowId)> AddShow(AddShowModel model)
    {
        var context = new ValidationContext(model, null, null);
        var results = new List<ValidationResult>();
        if(!Validator.TryValidateObject(model, context, results, true))
            return (false, null);
        var labelImage = await _imageService.Upload(model.LabelImage, "Shows");
        var images = new (Guid Id, ImageUploadStatusCode Status)[model.Images != null ? model.Images.Length: 0];
        if(images.Length > 0)
            for(int i = 0; i < images.Length; i++)
                images[i] =  await _imageService.Upload(model.Images![i], "ShowsExtraImages");
        bool isExtraImagesValid = images.Where(x => x.Status != ImageUploadStatusCode.Success)
            .Count() == 0;
        if(labelImage.Status == ImageUploadStatusCode.Success && isExtraImagesValid)
        {
            var show = new Show
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                LabelImage = labelImage.Id,
                Description = model.Description,
                Date = model.Date
            };
            var imagesForDb = images.Select(x => new ShowsImage{
                Id = x.Id,
                ShowId = show.Id
            })
            .ToArray();
            var transaction = await ExecuteInTransaction(async() => {
                await _context.Shows.AddAsync(show);
                await _context.ShowsImages.AddRangeAsync(imagesForDb);
                await _context.SaveChangesAsync();
            });
            if(transaction.Success)
                return (true, show.Id);
        }
        await _imageService.Delete(labelImage.Id, "Shows");
        foreach(var image in images)
            await _imageService.Delete(image.Id, "ShowsExtraImages");
        return (false, null);
    }
}