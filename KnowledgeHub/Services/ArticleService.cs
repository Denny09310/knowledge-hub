using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Services;

public class ArticleService(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task AddArticleAsync(Article article)
    {
        _db.Articles.Add(article);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(string id)
    {
        var article = await GetArticleAsync(id)
            ?? throw new InvalidOperationException($"Unable to delete article {id}.");

        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();
    }

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

    public async Task UpdateArticleAsync(string id, Article article)
    {
        if (id != article.Id)
        {
            throw new InvalidOperationException($"Unable to update article {id}.");
        }

        _db.Articles.Update(article);
        await _db.SaveChangesAsync();
    }
}