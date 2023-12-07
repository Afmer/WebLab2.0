using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Modules.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IDbFavorite _dbManager;
    public FavoriteService(IDbFavorite dbManager)
    {
        _dbManager = dbManager;
    }
    public async Task<(bool Success, List<ShowModel>? FavoriteShows)> AddFavorite(string login, Guid showId)
    {
        try
        {
            var result = await _dbManager.AddFavorite(login, showId);
            return(result.Success, result.Favorites);
        }
        catch
        {
            return (false, null);
        }
    }
    public async Task<(bool Success, List<ShowModel>? FavoriteShows)> DeleteFavorite(string login, Guid showId)
    {
        try
        {
            var result = await _dbManager.DeleteFavorite(login, showId);
            return(result.Success, result.Favorites);
        }
        catch
        {
            return (false, null);
        }
    }
    public List<ShowModel> ShowFavorite(string login)
    {
        return _dbManager.GetFavoriteShows(login);
    }
}