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

    public async Task<Status> Register(RegisterModel model)
    {
        var result = await ExecuteInTransaction(async () =>
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            if(Validator.TryValidateObject(model, context, results, true))
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
            }
            else throw new Exception("Invalid model");
        });
        if(result.Success)
        {
            return Status.Success;
        }
        else
        {
            return Status.UnknownError;
        }
    }

}