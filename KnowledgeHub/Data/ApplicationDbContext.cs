using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Reaction> Reactions { get; set; }

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

            entity.Property(e => e.Author)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Date)
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
    }
}