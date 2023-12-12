using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Controllers;

[ApiController]
[Route("api/[controller]/{action=Index}")]
public class ShowsController : ControllerBase
{
    private readonly IDbShows _dbManager;
    public ShowsController(IDbShows dbManager)
    {
        _dbManager = dbManager;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var result = _dbManager.GetAllShows();
        if(result.Status == Status.Success)
        {
            return Ok(result.Shows);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpGet]
    public IActionResult Show(string id)
    {
        Guid guid;
        if(Guid.TryParse(id, out guid))
        {
            var result = _dbManager.GetShow(new Guid(id));
            if(result.Status == GetShowStatus.Success)
            {
                return Ok(result.Show);
            }
            else
            {
                return NotFound();
            }
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromForm]AddShowModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _dbManager.AddShow(model);
            if(result.Success)
                return Ok(new {Success = true, ShowId = result.ShowId});
        }
        return BadRequest(new {Success = false});
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete([FromForm]DeleteShowModel model)
    {
        if(ModelState.IsValid)
        {
            var success = await _dbManager.DeleteShow(model);
            if(success)
                return Ok();
        }
        return BadRequest(model);
    }
}