using System.ComponentModel.DataAnnotations;
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
    public DbManagerService(ApplicationContext context, IHash hashService)
    {
        _context = context;
        _hashService = hashService;
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
    public async Task AddFeedback(FeedbackModel model, string login)
    {
        var feedback = new Feedback
        {
            UserId = login,
            Label = model.Label,
            Text = model.Text
        };
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();
    }
}