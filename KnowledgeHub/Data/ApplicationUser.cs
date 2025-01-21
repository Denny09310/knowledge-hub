using Microsoft.AspNetCore.Identity;

namespace KnowledgeHub.Data;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    #region Navigation Properties

    public ICollection<Article> Articles { get; set; } = [];
    public ICollection<Reaction> Reactions { get; set; } = [];

    #endregion Navigation Properties
}