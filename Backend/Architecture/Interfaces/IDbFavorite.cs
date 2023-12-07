using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IDbFavorite
{
    public Task<(bool Success, List<ShowModel>? Favorites)> AddFavorite(string login, Guid showId);
    public List<ShowModel> GetFavoriteShows(string login);
    public Task<(bool Success, List<ShowModel>? Favorites)> DeleteFavorite(string login, Guid showId);
}