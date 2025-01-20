using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KnowledgeHub.Components.Articles;

public class AuthorAuthorizationHandler : AuthorizationHandler<AuthorRequirement, Article>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorRequirement requirement, Article resource)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if ((resource?.UserId) != userId)
        {
            context.Fail();
            return Task.CompletedTask;
        }
         
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
