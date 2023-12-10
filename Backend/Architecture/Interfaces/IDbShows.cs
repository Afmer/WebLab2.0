using Weblab.Architecture.Enums;
using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IDbShows
{
    public (GetShowStatus Status, ShowModel? Show) GetShow(Guid id);
    public (Status Status, ShortShowModel[]? Shows) GetAllShows();
    public Task<(bool Success, Guid? ShowId)> AddShow(AddShowModel model);
}