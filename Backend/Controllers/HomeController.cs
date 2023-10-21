using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;

namespace Weblab.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IDbHome _dbHandler;
    public HomeController(IDbHome dbHandler)
    {
        _dbHandler = dbHandler;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var result = _dbHandler.GetHomeHtml();
        if(result.Status == GetHomeHtmlStatus.Success)
        {
            return Ok(new {html = result.Result});
        }
        else
        {
            return NotFound();
        }
    }
}
