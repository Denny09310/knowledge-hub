namespace KnowledgeHub.Data;

public class Article
{
    public string Author { get; set; } = default!;
    public string? Content { get; set; }
    public DateOnly CreatedAt { get; set; }
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int TotalReactions { get; set; }
    public string UserId { get; set; } = default!;
    public ArticleVisibility Visibility { get; set; }

    #region Navigation Properties

    public ICollection<Reaction> Reactions { get; set; } = [];

    public ApplicationUser User { get; set; } = default!;

    #endregion Navigation Properties
}

public enum ArticleVisibility
{
    Public,
    Private
}