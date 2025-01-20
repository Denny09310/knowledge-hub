using Markdig;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Services;

public class ArticleService(IConfiguration configuration, ApplicationDbContext db)
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .DisableHtml()
            .Build();

    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _db = db;

    public async Task<PagedResult<Article>> GetArticlesAsync(int offset = 0, int limit = 25)
    {
        var articles = await _db.Articles.OrderBy(x => x.Title)
            .Take(limit).Skip(offset)
            .ToListAsync();

        var count = await _db.Articles.CountAsync();

        return new(articles, count);
    }

    public async Task<Article?> LoadArticleAsync(string id)
    {
        var article = await _db.Articles.FindAsync([id]);

        if (article == null)
        {
            return null;
        }

        var uploadsPath = _configuration["Uploads:Path"] ?? throw new InvalidOperationException("Upload path not set.");
        var articlesPath = Environment.ExpandEnvironmentVariables(uploadsPath);
        var articleFilePath = Directory.EnumerateFiles(articlesPath)
            .FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == id);

        if (string.IsNullOrEmpty(articleFilePath))
            return null;

        article.Content = Markdown.ToHtml(await File.ReadAllTextAsync(articleFilePath), _pipeline);

        return article;
    }
}

public record struct PagedResult<T>(List<T> Items, int TotalItems);