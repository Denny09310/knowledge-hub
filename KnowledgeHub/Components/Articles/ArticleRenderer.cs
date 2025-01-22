using Markdig;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Components.Articles;

public class ArticleRenderer(IConfiguration configuration, ApplicationDbContext db)
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();

    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _db = db;

    public static string RenderArticle(string? content)
    {
        return Markdown.ToHtml(content ?? "", _pipeline);
    }

    public async Task<Article?> LoadArticleAsync(string id)
    {
        var article = await _db.Articles.Include(x => x.Reactions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (article == null)
        {
            return null;
        }

        var articlesFolder = ArticleFilesHelper.GetUploadFolderPath(_configuration, "Articles");
        var articleFilePath = ArticleFilesHelper.GenerateFilePath(articlesFolder, article.Id);

        if (!File.Exists(articleFilePath))
            return null;

        article.Content = RenderArticle(await File.ReadAllTextAsync(articleFilePath));

        return article;
    }
}