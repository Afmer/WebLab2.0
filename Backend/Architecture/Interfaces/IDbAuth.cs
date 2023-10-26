using Weblab.Architecture.Enums;
using Weblab.Models;

namespace Weblab.Architecture.Interfaces;

public interface IDbAuth
{
    public Task<Status> Register(RegisterModel model);
}