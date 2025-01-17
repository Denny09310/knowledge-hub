using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Icon)
                  .HasMaxLength(50);

            entity.Property(e => e.Count)
                  .HasDefaultValue(0);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.Content)
                  .IsRequired();

            entity.Property(e => e.CreatedAt)
                  .IsRequired();

            entity.Property(a => a.TotalReactions)
                  .HasDefaultValue(0);

            entity.HasMany(a => a.Categories)
                  .WithMany(c => c.Articles)
                  .UsingEntity<Dictionary<string, object>>(
                    "ArticleCategory",
                    j => j.HasOne<Category>()
                          .WithMany()
                          .HasForeignKey("CategoryId"),
                    j => j.HasOne<Article>()
                          .WithMany()
                          .HasForeignKey("ArticleId"));

            entity.HasMany(a => a.Reactions)
                  .WithOne(r => r.Article)
                  .HasForeignKey(r => r.ArticleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reaction>(entity =>
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(u => u.Password)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(u => u.Username)
                  .HasMaxLength(50); 

            entity.Property(u => u.CreatedAt)
                  .IsRequired();

            entity.Property(u => u.UpdatedAt)
                  .IsRequired();

            entity.Property(u => u.DeletedAt)
                  .IsRequired(false);

            entity.Ignore(u => u.IsDeleted);

            entity.HasMany(u => u.Articles)
                  .WithOne(a => a.Author)
                  .HasForeignKey(a => a.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}