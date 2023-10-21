using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Modules.DB;

namespace Weblab.Modules.Services;

public class DbManagerService : IDbHome
{
    private readonly ApplicationContext _context;
    public DbManagerService(ApplicationContext context)
    {
        _context = context;
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
}