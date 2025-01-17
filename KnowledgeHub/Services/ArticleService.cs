using KnowledgeHub.Data;
using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Services;

public class ArticleService(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public Task<List<Article>> GetFeaturedArticlesAsync()
    {
        return _db.Articles.OrderByDescending(x => x.TotalReactions)
            .ToListAsync();
    }

    public Task<Article?> GetArticleAsync(string  id)
    {
        return _db.Articles.FirstOrDefaultAsync(x => x.Id == id);
    }
}
