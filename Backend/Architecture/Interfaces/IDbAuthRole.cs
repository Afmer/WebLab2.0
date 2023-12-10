using Weblab.Architecture.Enums;

namespace Weblab.Architecture.Interfaces;

public interface IDbAuthRole
{
    public Task<Role?> GetRole(string login);
}