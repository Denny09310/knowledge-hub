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

        var articlesFolder = ArticleFilesHelper.GetUploadFolderPath(_configuration, "Articles");
        var articleFilePath = ArticleFilesHelper.GenerateFilePath(articlesFolder, article.Id);
        
        await File.WriteAllTextAsync(articleFilePath, article.Content);

        return article;
    }

    public async Task SetReactionAsync(Article article, string userId, ReactionType type)
    {
        var reaction = await _db.Reactions.FirstOrDefaultAsync(x => x.ArticleId == article.Id && x.UserId == userId);

        if (reaction == null)
        {
            // Add a new reaction
            reaction = new()
            {
                ArticleId = article.Id,
                UserId = userId,
                Type = type,
                Timestamp = DateTime.UtcNow
            };
            await _db.Reactions.AddAsync(reaction);

            article.TotalReactions++;
            _db.Articles.Update(article);
        }
        else if (reaction.Type == type)
        {
            // Remove existing reaction
            _db.Reactions.Remove(reaction);
            article.TotalReactions = Math.Max(0, article.TotalReactions - 1);
        }
        else
        {
            // Update reaction type
            reaction.Type = type;
            reaction.Timestamp = DateTime.UtcNow;
            _db.Reactions.Update(reaction);
        }

        await _db.SaveChangesAsync();
    }


    public async Task DeleteArticleAsync(string articleId)
    {
        var article = await _db.Articles.FindAsync(articleId);
        if (article == null)
        {
            return;
        }
        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();

        var articlesFolder = ArticleFilesHelper.GetUploadFolderPath(_configuration, "Articles");
        var articleFilePath = ArticleFilesHelper.GenerateFilePath(articlesFolder, article.Id);

        File.Delete(articleFilePath);
    }

    public async Task<Article?> GetArticleAsync(string articleId)
    {
        var article = await _db.Articles.FindAsync(articleId);

        if (article == null)
        {
            return null;
        }

        var articlesFolder = ArticleFilesHelper.GetUploadFolderPath(_configuration, "Articles");
        var articleFilePath = ArticleFilesHelper.GenerateFilePath(articlesFolder, article.Id);

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

        var articlesFolder = ArticleFilesHelper.GetUploadFolderPath(_configuration, "Articles"); 
        var articleFilePath = ArticleFilesHelper.GenerateFilePath(articlesFolder, article.Id);
        
        await File.WriteAllTextAsync(articleFilePath, article.Content);

        return article;
    }
}

public record struct PagedResult<T>(List<T> Items, int TotalItems);