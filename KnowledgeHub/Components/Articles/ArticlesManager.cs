using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Components.Articles;

public class ArticlesManager(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task<PagedResult<Article>> GetArticlesAsync(int offset = 0, int limit = 25)
    {
        var articles = await _db.Articles.OrderBy(x => x.Title)
            .Take(limit).Skip(offset)
            .Where(x => x.Visibility == ArticleVisibility.Public)
            .ToListAsync();

        var count = await _db.Articles.CountAsync();

        return new(articles, count);
    }

    public async Task<Article?> GetArticleAsync(string articleId)
    {
        return await _db.Articles.FindAsync(articleId);
    }
}

public record struct PagedResult<T>(List<T> Items, int TotalItems);