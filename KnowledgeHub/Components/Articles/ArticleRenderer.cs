﻿using Markdig;

namespace KnowledgeHub.Components.Articles;

public class ArticleRenderer(IConfiguration configuration, ApplicationDbContext db)
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();

    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _db = db;

    public async Task<Article?> LoadArticleAsync(string id)
    {
        var article = await _db.Articles.FindAsync([id]);

        if (article == null)
        {
            return null;
        }

        var articlesFolder = _configuration["Uploads:Articles"] ?? throw new InvalidOperationException("Upload path not set."); ;
        Directory.CreateDirectory(articlesFolder);

        var articleFilePath = Directory.EnumerateFiles(articlesFolder)
            .FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == id);

        if (string.IsNullOrEmpty(articleFilePath))
            return null;

        article.Content = Markdown.ToHtml(await File.ReadAllTextAsync(articleFilePath), _pipeline);

        return article;
    }
}