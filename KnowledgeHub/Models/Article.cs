namespace KnowledgeHub.Models;

public class Article
{
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;

    #region Computed Properties

    public int TotalReactions { get; set; } = 0;

    #endregion Computed Properties

    #region Foreign Properties

    public string AuthorId { get; set; } = default!;
    public User Author { get; set; } = default!;

    #endregion Foreign Properties

    #region Navigation Properties

    public ICollection<Reaction> Reactions { get; set; } = [];

    #endregion Navigation Properties
}