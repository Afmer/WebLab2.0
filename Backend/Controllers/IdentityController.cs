using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Models;
using Weblab.Modules.Services;

namespace Weblab.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly IJwtConfig _jwtConfig;
    private readonly IDbAuth _dbManager;
    private readonly IIdentityService _identityService;
    public IdentityController(IJwtConfig jwtConfig, IDbAuth dbManager, IIdentityService identityService)
    {
        _jwtConfig = jwtConfig;
        _dbManager = dbManager;
        _identityService = identityService;
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]RegisterModel model)
    {
        if(ModelState.IsValid)
        {
            var registerStatus = await _dbManager.Register(model);
            if(registerStatus == Status.Success)
            {
                var token = GenerateJwtToken(model.Login);
                HttpContext.Response.Cookies.Append(CookieNames.Jwt, token, 
                new CookieOptions
                {
                        MaxAge = TimeSpan.FromDays(1)
                });
                return Ok();
            }
            else return BadRequest();
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
    private string GenerateJwtToken(string login)
    {
        var credentials = new SigningCredentials(_jwtConfig.SecurityKey, _jwtConfig.Algorithm);
        var claims = new[]
        {
            new Claim(JwtClaimsConstant.Login, login)
        };
        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}