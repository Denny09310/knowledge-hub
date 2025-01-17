using KnowledgeHub.Data;
using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Services;

public class CategoryService(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public Task<List<Category>> GetCategoriesAsync()
    {
        return _db.Categories.ToListAsync();
    }
}
