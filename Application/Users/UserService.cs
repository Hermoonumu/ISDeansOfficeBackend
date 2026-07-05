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

    public async Task PatchUserAsync(string guid, JsonPatchDocument<UserUpdateDTO> Patch)
    {
        User? user = await _userRepo.GetUserByGuidAsync(Guid.Parse(guid)) ??
        throw new UserDoesntExistException("No such user");
        UserUpdateDTO bfPatch = new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Position = user.Position,
            BirthDate = user.BirthDate,
            ProgramId = user.ProgramId
        };
        Patch.ApplyTo(bfPatch, (err) =>
        {
            throw new UpdateFailedException($"User update failed.\nAmended object: {err.AffectedObject.GetType()}\nError msg: {err.ErrorMessage}");
        });
        user.FirstName = bfPatch.FirstName; user.LastName = bfPatch.LastName;
        user.Username = bfPatch.Username; user.Position = bfPatch.Position;
        user.BirthDate = bfPatch.BirthDate; user.ProgramId = bfPatch.ProgramId;
        await _userRepo.PersistChangesAsync();
    }

    public async Task RemoveUserAsync(string guid)
    {
        User user = await _userRepo.GetUserByGuidAsync(Guid.Parse(guid)) ??
                        throw new UserDoesntExistException("No such user");
        await _userRepo.RemoveUserAsync(user);
    }
}