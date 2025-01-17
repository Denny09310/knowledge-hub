namespace KnowledgeHub.Models;

public class Category
{
    public int Count { get; set; }
    public string Icon { get; set; } = default!;
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    #region Navigation Properties

    public ICollection<Article> Articles { get; set; } = [];

    #endregion Navigation Properties
}