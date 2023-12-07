using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IFavoriteService
{
    public Task<(bool Success, List<ShowModel>? FavoriteShows)> AddFavorite(string login, Guid showId);
    public Task<(bool Success, List<ShowModel>? FavoriteShows)> DeleteFavorite(string login, Guid showId);
}