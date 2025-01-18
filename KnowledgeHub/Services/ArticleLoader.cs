using KnowledgeHub.Models;
using Markdig;
using Markdig.Extensions.Yaml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnowledgeHub.Services;

public class ArticleLoader(IConfiguration configuration)
{
    private static readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseYamlFrontMatter()
            .DisableHtml()
            .Build();

    private readonly IConfiguration _configuration = configuration;

    public async Task<Article?> LoadArticleAsync(string id)
    {
        var uploadsPath = _configuration["Uploads:Path"] ?? throw new InvalidOperationException("Upload path not set.");
        var articlesPath = Environment.ExpandEnvironmentVariables(uploadsPath);
        var articleFilePath = Directory.EnumerateFiles(articlesPath)
            .FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == id);

        if (string.IsNullOrEmpty(articleFilePath))
            return null;

        var articleContent = await File.ReadAllTextAsync(articleFilePath);
        var articleMarkdown = Markdown.Parse(articleContent, _pipeline);

        var articleMetadataBlock = articleMarkdown
            .OfType<YamlFrontMatterBlock>()
            .FirstOrDefault();

        if (articleMetadataBlock == null)
            return null;

        articleMarkdown.Remove(articleMetadataBlock);

        var articleMetadata = _deserializer.Deserialize<Article>(articleMetadataBlock.Lines.ToString());
        var htmlContent = articleMarkdown.ToHtml();

        return new Article
        {
            Title = articleMetadata.Title,
            Author = articleMetadata.Author,
            Date = articleMetadata.Date,
            Content = htmlContent
        };
    }
}