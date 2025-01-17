namespace KnowledgeHub.Models;

public class Article
{
    public string Author { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime Date { get; set; }
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;

    #region Computed Properties

    public int TotalReactions { get; set; } = 0;

    #endregion Computed Properties

    #region Foreign Properties

    public string AuthorId { get; set; } = default!;
    public User User { get; set; } = default!;

    #endregion Foreign Properties

    #region Navigation Properties

    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Reaction> Reactions { get; set; } = [];

    #endregion Navigation Properties
}