using Microsoft.AspNetCore.Authorization;
using Weblab.Architecture.Enums;

namespace Weblab.Modules.AuthorizationRequirement;
public class RoleHierarсhyRequirement : IAuthorizationRequirement
{
    public Role Role {get; set;}
    public RoleHierarсhyRequirement(Role role) => Role = role;
}