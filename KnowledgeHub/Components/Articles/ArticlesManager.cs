using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Components.Articles;

public class ArticlesManager(ApplicationDbContext db, IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _db = db;

    public async Task<Article> CreateArticleAsync(Article article)
    {
        await _db.Articles.AddAsync(article);
        await _db.SaveChangesAsync();

        var articlesFolder = _configuration["Uploads:Articles"] ?? throw new InvalidOperationException("Upload path not set.");
        Directory.CreateDirectory(articlesFolder);

        var articleFilePath = Path.Combine(articlesFolder, $"{article.Id}.md");
        await File.WriteAllTextAsync(articleFilePath, article.Content);

        return article;
    }

    public async Task<Article?> GetArticleAsync(string articleId)
    {
        var article = await _db.Articles.FindAsync(articleId);

        if (article == null)
        {
            return null;
        }

        var articlesFolder = _configuration["Uploads:Articles"] ?? throw new InvalidOperationException("Upload path not set.");
        Directory.CreateDirectory(articlesFolder);

        var articleFilePath = Path.Combine(articlesFolder, $"{article.Id}.md");
        article.Content = await File.ReadAllTextAsync(articleFilePath);

        return article;
    }

    public async Task<PagedResult<Article>> GetArticlesAsync(int offset = 0, int limit = 25)
    {
        var articles = await _db.Articles.OrderBy(x => x.Title)
            .Take(limit).Skip(offset)
            .Where(x => x.Visibility == ArticleVisibility.Public)
            .ToListAsync();

        var count = await _db.Articles.CountAsync();

        return new(articles, count);
    }

    public async Task<Article?> UpdateArticleAsync(Article article)
    {
       _db.Articles.Update(article);
        await _db.SaveChangesAsync();

        var articlesFolder = _configuration["Uploads:Articles"] ?? throw new InvalidOperationException("Upload path not set.");
        var articleFilePath = Path.Combine(articlesFolder, $"{article.Id}.md");
        await File.WriteAllTextAsync(articleFilePath, article.Content);

        return article;
    }
}

public record struct PagedResult<T>(List<T> Items, int TotalItems);