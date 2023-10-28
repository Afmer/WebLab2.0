using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Models;

namespace Weblab.Modules.Services;

public class IdentityService : IIdentityService
{
    private readonly IDbAuth _dbManager;
    private readonly IJwtConfig _jwtConfig;
    private readonly IHash _hashService;
    public IdentityService(IDbAuth dbManager, IJwtConfig jwtConfig, IHash hashService)
    {
        _dbManager = dbManager;
        _jwtConfig = jwtConfig;
        _hashService = hashService;
    }
    public async Task<LoginStatus> Login(LoginModel model, HttpContext context)
    {
        var userInfo = await _dbManager.GetUser(model.Login);
        if(userInfo.Status == GetUserStatus.NotFound)
            return LoginStatus.NotFound;
        if(!_hashService.IsPasswordValid(model.Password, userInfo.User!.Salt, userInfo.User!.PasswordHash))
            return LoginStatus.InvalidPassword;
        var token = GenerateJwtToken(model.Login);
        context.Response.Cookies.Append(CookieNames.Jwt, token, 
        new CookieOptions
        {
            MaxAge = TimeSpan.FromDays(1)
        });
        return LoginStatus.Success;
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
            expires: DateTime.UtcNow.AddDays(3),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}