using Microsoft.AspNetCore.Identity;

namespace KnowledgeHub.Data;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    #region Navigation Properties

    public ICollection<Article> Articles { get; set; } = [];

    #endregion Navigation Properties
}