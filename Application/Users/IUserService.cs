using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task AddUserAsync(NewUserDTO nuDTO);
    public Task<User?> GetUserByIdAsync(string guid);
    public Task PatchUserAsync(User user, JsonPatchDocument<User> Patch);
    public Task RemoveUserAsync(string guid);
}