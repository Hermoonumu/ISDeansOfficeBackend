using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public class UserService(IUserRepository _userRepo) : IUserService
{
    public async Task AddUserAsync(User user)
    {
        await _userRepo.AddUserAsync(user);
    }

    public async Task AddUserAsync(NewUserDTO nuDTO)
    {
        User user = UserMapper.DTOToUser(nuDTO);
        await _userRepo.AddUserAsync(user);
    }

    public async Task<User?> GetUserByIdAsync(string guid)
    {
        return await _userRepo.GetUserByGuidAsync(guid);
    }

    public async Task PatchUserAsync(User user, JsonPatchDocument<User> Patch)
    {
        Patch.ApplyTo(user, (err) =>
        {
            throw new UpdateFailedException($"User update failed.\nAmended object: {err.AffectedObject.GetType()}\nError msg: {err.ErrorMessage}");
        });
        await _userRepo.PersistChanges();
    }

    public async Task RemoveUserAsync(string guid)
    {
        User user = await _userRepo.GetUserByGuidAsync(guid) ??
                        throw new UserDoesntExistException("No such user");
        await _userRepo.RemoveUserAsync(user);
    }
}