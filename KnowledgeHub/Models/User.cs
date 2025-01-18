namespace KnowledgeHub.Models;

public class User
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public string Email { get; set; } = default!;
    public string Id { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? Username { get; set; }

    #region Computed Properties

    public bool IsDeleted => DeletedAt != null;

    #endregion Computed Properties

    #region Navigation Properties

    public ICollection<Article> Articles { get; set; } = [];

    #endregion Navigation Properties
}