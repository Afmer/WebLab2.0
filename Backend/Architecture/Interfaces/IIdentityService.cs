using Weblab.Architecture.Enums;
using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IIdentityService
{
    public Task<LoginStatus> Login(LoginModel model, HttpContext context);
    public Task<RegisterStatus> Register(RegisterModel model, HttpContext context);
    public Task<UserIdentityBaseModel?> GetUserIdentityBaseInfo(string login);
}