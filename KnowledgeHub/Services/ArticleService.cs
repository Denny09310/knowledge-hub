using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Services;

public class ArticleService(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public Task<Article?> GetArticleAsync(string id)
    {
        return _db.Articles.Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Article>> GetFeaturedArticlesAsync()
    {
        return _db.Articles.Include(x => x.Author)
            .OrderByDescending(x => x.TotalReactions)
            .Take(3)
            .ToListAsync();
    }
}