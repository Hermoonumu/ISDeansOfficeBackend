using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class UserRepository(SystemDbContext _db) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetUserByGuidAsync(Guid guid)
    {
        return await _db.Users.Where(u => u.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _db.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersByPositionAsync(Position position)
    {
        return await _db.Users.Where(u => u.Position == position).ToListAsync();
    }

    public async Task RemoveUserAsync(User user)
    {
        await _db.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync();
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        User? user = await _db.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        if (user is null) return false;
        return true;
    }

    public async Task PersistChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}