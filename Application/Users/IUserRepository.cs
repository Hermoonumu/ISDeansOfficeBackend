using DeanInfoSystem.Domain;
using Microsoft.OpenApi;

namespace DeanInfoSystem.Application.Users;


public interface IUserRepository
{
    public Task AddUserAsync(User user);
    public Task<User?> GetUserByGuidAsync(Guid guid);
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<List<User>> GetUsersByPositionAsync(Position position);
    public Task RemoveUserAsync(User user);
    public Task<bool> IsUsernameTaken(string username);
    public Task<List<User>> GetAllUsersPageAsync(int page, int take);
    public Task<List<User>> GetAllUsersByPositionPageAsync(Position pos, int page, int take);
    public Task<List<User>> GetAllUsersInProgramAsync(Guid ProgId);
}