using KnowledgeHub.Data;
using KnowledgeHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KnowledgeHub.Services;

public class UserContext(ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db;

    public async Task AddUserAsync(User user)
    {
        user.Password = Hasher.EnhancedHashPassword(user.Password);

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(HttpContext context)
    {
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return null;
        }

        return await _db.Users.FindAsync(userId);
    }

    public async Task<User?> SignInWithPassword(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !Hasher.EnhancedVerify(user.Password, password))
        {
            return null;
        }
        return user;
    }
}