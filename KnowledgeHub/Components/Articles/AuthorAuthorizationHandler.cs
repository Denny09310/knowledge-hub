using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KnowledgeHub.Components.Articles;

public class AuthorRequirement : IAuthorizationRequirement;

internal class AuthorAuthorizationHandler(IServiceScopeFactory factory) : AuthorizationHandler<AuthorRequirement, Article>
{
    private readonly IServiceScopeFactory _factory = factory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorRequirement requirement, Article resource)
    {
        using var scope = _factory.CreateScope();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.GetUserAsync(context.User);

        if (user == null)
        {
            context.Fail();
            return;
        }

        if (string.IsNullOrEmpty(user.Id))
        {
            context.Fail();
            return;
        }

        if (resource?.UserId != user.Id)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}