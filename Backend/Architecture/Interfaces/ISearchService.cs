using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface ISearchService
{
    public ShowSearchResultModel[] SearchShows(string query);
}