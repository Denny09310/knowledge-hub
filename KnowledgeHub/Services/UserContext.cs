using KnowledgeHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KnowledgeHub.Services;

public class UserContext(ApplicationDbContext db)
{
    private static readonly PasswordHasher<User> _hasher = new();

    private readonly ApplicationDbContext _db = db;

    public async Task AddUserAsync(User user)
    {
        user.Password = _hasher.HashPassword(user, user.Password);

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
        if (user == null)
        {
            return null;
        }

        var result = _hasher.VerifyHashedPassword(user, user.Password, password);
        if (result is not PasswordVerificationResult.Success)
        {
            return null;
        }

        return user;
    }
}