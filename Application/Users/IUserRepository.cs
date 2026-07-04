using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Users;


public interface IUserRepository
{
    public Task AddUserAsync(User user);
    public Task<User?> GetUserByGuidAsync(string guid);
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<List<User>> GetUsersByPositionAsync(Position position);
    public Task RemoveUserAsync(User user);
    public Task<bool> IsUsernameTaken(string username);
    public Task PersistChangesAsync();
}