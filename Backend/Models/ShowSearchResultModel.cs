namespace Weblab.Models;

public class ShowSearchResultModel
{
    public Guid ShowId {get; protected set;}
    public string Name {get; protected set;}
    public ShowSearchResultModel(Guid showId, string name)
    {
        ShowId = showId;
        Name = name;
    }
}