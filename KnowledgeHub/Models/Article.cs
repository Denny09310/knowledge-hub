namespace KnowledgeHub.Models;

public class Article
{
    public string Author { get; set; } = default!;
    public DateOnly Date { get; set; }
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Content { get; set; }
}