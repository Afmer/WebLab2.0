using Weblab.Architecture.Enums;
using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IIdentityService
{
    public Task<LoginStatus> Login(LoginModel model, HttpContext context);
}