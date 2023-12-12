using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Modules.Services;

public class SphinxService : ISearchService
{
    private readonly ISphinxConnector _sphinxConnector;
    public SphinxService(ISphinxConnector sphinxConnector)
    {
        _sphinxConnector = sphinxConnector;
    }
    private List<object[]> Search(string query, string index, string[] attributes)
    {
        string attributesStr = "";
        for(int i = 0; i < attributes.Length - 1; i++)
            attributesStr += attributes[i] + ", ";
        attributesStr += attributes[^1];
        var result = _sphinxConnector.GetData($"SELECT {attributesStr} FROM {index} WHERE MATCH('{query}')");
        return result;
    }
    public ShowSearchResultModel[] SearchShows(string query)
    {
        var data = Search(query, "ShowsIndex", new string[]{"ShowId", "Name"});
        var result = data.Select(x => new ShowSearchResultModel(new Guid((string)x[0]), (string)x[1])).ToArray();
        return result;
    }
}