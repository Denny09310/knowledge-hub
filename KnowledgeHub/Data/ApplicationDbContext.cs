using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Name Overrides

        builder.Entity<ApplicationUser>().ToTable("asp_net_users");
        builder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
        builder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
        builder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        builder.Entity<IdentityRole>().ToTable("asp_net_roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");

        #endregion

        builder.Entity<Reaction>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id)
                  .IsRequired()
                  .HasMaxLength(36);

            entity.Property(r => r.ArticleId)
                  .IsRequired()
                  .HasMaxLength(36);

            entity.Property(r => r.Type)
                  .IsRequired()
                  .HasConversion<string>();

            entity.Property(r => r.UserId)
                  .HasMaxLength(36);

            entity.Property(r => r.Timestamp)
                  .IsRequired();
        });

        builder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Ignore(e => e.Content);

            entity.Property(e => e.CreatedAt)
                  .IsRequired();

            entity.Property(a => a.TotalReactions)
                  .HasDefaultValue(0);
            
            entity.Property(a => a.Visibility)
                  .HasConversion<string>();

            entity.HasMany(a => a.Reactions)
                  .WithOne(r => r.Article)
                  .HasForeignKey(r => r.ArticleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.CreatedAt)
                  .IsRequired();

            entity.Property(u => u.UpdatedAt)
                  .IsRequired();

            entity.Property(u => u.DeletedAt)
                  .IsRequired(false);

            entity.HasMany(a => a.Articles)
                  .WithOne(r => r.User)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}