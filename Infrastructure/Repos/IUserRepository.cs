using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Infrastructure.Repos.Interface;


public interface IUserRepository
{
    public Task AddUserAsync(User user);
    public Task<User> GetUserByGuidAsync(string guid);
    public Task<User> GetUserByUsernameAsync(string username);
    public Task<List<User>> GetUsersByPositionAsync(Position position);
    public Task RemoveUserAsync(User user);
    public Task UpdateTrackedUserAsync();


}