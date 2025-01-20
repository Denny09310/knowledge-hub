using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Components.Articles;

public class ArticlesManager(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task<PagedResult<Article>> GetArticlesAsync(int offset = 0, int limit = 25)
    {
        var articles = await _db.Articles.OrderBy(x => x.Title)
            .Take(limit).Skip(offset)
            .ToListAsync();

        var count = await _db.Articles.CountAsync();

        return new(articles, count);
    }
}

public record struct PagedResult<T>(List<T> Items, int TotalItems);