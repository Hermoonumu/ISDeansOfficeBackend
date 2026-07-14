using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class UserRepository(SystemDbContext _db) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task<User?> GetUserByGuidAsync(Guid guid)
    {
        return await _db.Users.Include(u => u.Program)
                                .ThenInclude(p => p.Department)
                                .Where(u => u.Id == guid)
                                .FirstOrDefaultAsync();
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

    public async Task<List<User>> GetAllUsersPageAsync(int page, int take)
    {
        return await _db.Users.Skip(page * take).Take(take).ToListAsync();
    }

    public async Task<List<User>> GetAllUsersByPositionPageAsync(Position pos, int page, int take)
    {
        return await _db.Users.Where(u => u.Position == pos)
                                .Skip(page * take)
                                .Take(take)
                                .ToListAsync();
    }

    public async Task<List<User>> GetAllUsersInProgramAsync(Guid ProgId)
    {
        return await _db.Users.Where(u => u.ProgramId == ProgId).ToListAsync();
    }
}