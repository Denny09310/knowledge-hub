namespace KnowledgeHub.Models;

public class Article
{
    public string Author { get; set; } = default!;
    public string? Content { get; set; }
    public DateOnly CreatedAt { get; set; }
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int TotalReactions { get; set; }

    #region Navigation Properties

    public ICollection<Reaction> Reactions { get; set; } = [];


    #endregion Navigation Properties
}