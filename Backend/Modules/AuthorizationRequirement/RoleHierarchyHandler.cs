using Microsoft.AspNetCore.Authorization;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;

namespace Weblab.Modules.AuthorizationRequirement;
public class RoleHierarсhyHandler : AuthorizationHandler<RoleHierarсhyRequirement>
{
    protected IDbAuthRole _db;
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleHierarсhyRequirement requirement)
    {
        if(context.User.Identity != null && context.User.Identity.Name != null)
        {
            var login = context.User.Identity.Name;
            var usersRole = await _db.GetRole(login);
            if(usersRole != null)
            {
                if(usersRole <= requirement.Role)
                    context.Succeed(requirement);
                else
                    context.Fail();
                return;
            }
        }
        context.Fail();
    }
    public RoleHierarсhyHandler(IDbAuthRole db) => _db = db;
}