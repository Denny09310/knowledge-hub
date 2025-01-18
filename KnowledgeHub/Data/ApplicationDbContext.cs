using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<Article>(entity =>
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

            entity.HasMany(a => a.Reactions)
                  .WithOne(r => r.Article)
                  .HasForeignKey(r => r.ArticleId)
                  .OnDelete(DeleteBehavior.Cascade);
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
        });
    }
}