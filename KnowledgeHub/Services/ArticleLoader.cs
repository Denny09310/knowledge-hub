using KnowledgeHub.Models;
using Markdig;
using Markdig.Extensions.Yaml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnowledgeHub.Services;

public class ArticleLoader(IWebHostEnvironment webHostEnvironment)
{
    private static readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseYamlFrontMatter()
            .DisableHtml()
            .Build();

    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<Article?> LoadArticleAsync(string id)
    {
        var articlesPath = Path.Combine(_webHostEnvironment.WebRootPath, "articles");
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