using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Infrastructure.Repos;


public class UserRepository(SystemDbContext _db) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {

    }

    public async Task<User> GetUserByGuidAsync(string guid)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetUsersByPositionAsync(Position position)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTrackedUserAsync()
    {
        throw new NotImplementedException();
    }
}