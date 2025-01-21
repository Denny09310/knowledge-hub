namespace KnowledgeHub.Data;

public class Reaction
{
    public string ArticleId { get; set; } = default!;
    public string Id { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public ReactionType Type { get; set; }
    public string UserId { get; set; } = default!;

    #region Navigation Properties

    public Article Article { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    #endregion Navigation Properties
}

public enum ReactionType
{
    Like,
    Dislike,
    Clapping,
    Fire,
    Love
}