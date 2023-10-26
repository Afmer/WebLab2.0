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
    public IdentityController(IJwtConfig jwtConfig, IDbAuth dbManager)
    {
        _jwtConfig = jwtConfig;
        _dbManager = dbManager;
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