using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Controllers;

[ApiController]
[Route("api/[controller]/{action}")]
[Authorize]
public class FavoriteShowController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;
    public FavoriteShowController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }
    [HttpGet]
    public async Task<IActionResult> Add(Guid showId)
    {
        if(ModelState.IsValid && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
        {
            string login = HttpContext.User.Identity.Name;
            var result = await _favoriteService.AddFavorite(login, showId);
            if(result.Success)
            {
                return Ok(result.FavoriteShows);
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
    }
    [HttpGet]
    public async Task<IActionResult> Delete(Guid showId)
    {
        if(ModelState.IsValid && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
        {
            string login = HttpContext.User.Identity.Name;
            var result = await _favoriteService.DeleteFavorite(login, showId);
            if(result.Success)
            {
                return Ok(result.FavoriteShows);
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
    }
}