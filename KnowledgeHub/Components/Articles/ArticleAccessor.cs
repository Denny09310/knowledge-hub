using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KnowledgeHub.Components.Articles;

public class ArticleAccessor(AuthenticationStateProvider provider)
{
    private readonly AuthenticationStateProvider _provider = provider;

    public async Task<bool> IsUserAuthor(Article? article)
    {
        var currentUserId = await GetCurrentUserId();
        return currentUserId == article?.UserId;
    }

    private async Task<string?> GetCurrentUserId()
    {
        var authState = await _provider.GetAuthenticationStateAsync();
        return authState?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}