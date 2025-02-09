using Microsoft.AspNetCore.Authorization;
using Roomly.Users.Infrastructure.Auth;

namespace Roomly.Users.Infrastructure.Handlers;

public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        if (context.User.IsInRole(requirement.Role))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}