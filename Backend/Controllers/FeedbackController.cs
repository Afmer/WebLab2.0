using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Controllers;

[ApiController]
[Route("api/[controller]/{action=Index}")]
public class FeedbackController : ControllerBase
{
    private readonly IDbFeedback _dbManager;
    public FeedbackController(IDbFeedback dbManager)
    {
        _dbManager = dbManager;
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Index(FeedbackModel model)
    {
        if(ModelState.IsValid && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
        {
            var result = await _dbManager.AddFeedback(model, HttpContext.User.Identity.Name);
            if(result)
                return Ok();
            else
                return BadRequest();
        }
        else
        {
            return BadRequest();
        }
    }
}