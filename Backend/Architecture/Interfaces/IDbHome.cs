using Weblab.Architecture.Enums;

namespace Weblab.Architecture.Interfaces;

public interface IDbHome
{
    public (GetHomeHtmlStatus Status, string? Result) GetHomeHtml();
}