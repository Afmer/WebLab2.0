using Microsoft.AspNetCore.Mvc;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]RegisterModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _identityService.Register(model, HttpContext);
            if(result == RegisterStatus.UserExists)
                return BadRequest("User exists");
            else if(result == RegisterStatus.UnknownError)
                return BadRequest("Unknown error");
            else
                return Ok();
        }
        else return BadRequest(ModelState);
    }
    public async Task<IActionResult> Login([FromBody]LoginModel model)
    {
        var result = await _identityService.Login(model, HttpContext);
        if(result == LoginStatus.NotFound)
            return BadRequest("not found");
        else if(result == LoginStatus.InvalidPassword)
            return BadRequest("invalid password");
        else if(result == LoginStatus.Success)
            return Ok();
        else 
            return BadRequest();
    }
    [HttpGet]
    public IActionResult Logout()
    {
        _identityService.Logout(HttpContext);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> WhoIAm()
    {
        if(HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
        {
            var user = await _identityService.GetUserIdentityBaseInfo(HttpContext.User.Identity.Name);
            if(user is null)
                return Unauthorized();
            var data = new {login = user.Login, role = user.Role};
            return Ok(data);
        }
        else return Unauthorized();
    }
}