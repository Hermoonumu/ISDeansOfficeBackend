using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Subjects;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public class UserService(IUserRepository _userRepo,
                         IEducatorCurriculumRepository _prsuRepo,
                         ICurriculaRepository _currRepo) : IUserService
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

    public async Task AssignProfToCurriculumAsync(Guid UserId, Guid CurrId)
    {
        User? user = await _userRepo.GetUserByGuidAsync(UserId) ??
        throw new UserDoesntExistException("No such user");
        Curriculum? curriculum = await _currRepo.GetCurriculumByIdAsync(CurrId) ??
        throw new SubjectDoesntExistException("No such subject");
        if (user.Position != Position.Educator)
            throw new PositionException("The user is not an educator");
        if (await _prsuRepo.IsAlreadyAssigned(UserId, CurrId))
            throw new UpdateFailedException("The user has this subject assigned already");
        await _prsuRepo.AssignUserToCurriculumAsync(UserId, CurrId);
    }

    public async Task DismissUserFromCurriculumAsync(Guid UserId, Guid CurriculumId)
    {
        User? user = await _userRepo.GetUserByGuidAsync(UserId) ??
        throw new UserDoesntExistException("No such user");
        Curriculum? curriculum = await _currRepo.GetCurriculumByIdAsync(CurriculumId) ??
        throw new SubjectDoesntExistException("No such subject");
        if (user.Position != Position.Educator)
            throw new PositionException("The user is not an educator");
        if (await _prsuRepo.IsAlreadyAssigned(UserId, CurriculumId))
            await _prsuRepo.DismissUserFromCurriculumAsync(UserId, CurriculumId);
        throw new UpdateFailedException("The user is not assigned to this subject");
    }

    public async Task<List<UserDTO>> GetAllUsersByPositionPageAsync(Position pos, int page, int take)
    {
        List<User> users = await _userRepo.GetAllUsersByPositionPageAsync(pos, page, take);
        return [.. users.Select(UserMapper.UserToDTO)];
    }

    public async Task<List<UserDTO>> GetAllUsersPageAsync(int page, int take)
    {
        List<User> users = await _userRepo.GetAllUsersPageAsync(page, take);
        return [.. users.Select(UserMapper.UserToDTO)];
    }

    public async Task<List<Curriculum>> GetCurriculaAssignedAsync(Guid UserId)
    {
        User? user = await _userRepo.GetUserByGuidAsync(UserId) ??
        throw new UserDoesntExistException("No such user");
        return await _currRepo.GetCurriculaAssignedAsync(UserId);
    }

    public async Task<User?> GetUserByGuidAsync(Guid UserId)
    {
        return await _userRepo.GetUserByGuidAsync(UserId) ??
        throw new UserDoesntExistException("No such user");
    }

    public async Task PatchUserAsync(Guid guid, JsonPatchDocument<UserUpdateDTO> Patch)
    {
        User? user = await _userRepo.GetUserByGuidAsync(guid) ??
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

    public async Task RemoveUserAsync(Guid guid)
    {
        User user = await _userRepo.GetUserByGuidAsync(guid) ??
                        throw new UserDoesntExistException("No such user");
        await _userRepo.RemoveUserAsync(user);
    }
}