using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Subjects;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public class UserService(IUserRepository _userRepo,
                         IProfessorSubjectRepository _prsuRepo,
                         ISubjectRepository _subjRepo) : IUserService
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

    public async Task AssignProfToSubjectAsync(string SubjId, string UserId)
    {
        Guid SubjGuid = Guid.Parse(SubjId);
        Guid UserGuid = Guid.Parse(UserId);
        User? user = await _userRepo.GetUserByGuidAsync(UserGuid) ??
        throw new UserDoesntExistException("No such user");
        Subject? subject = await _subjRepo.GetSubjectByGuidAsync(SubjGuid) ??
        throw new SubjectDoesntExistException("No such subject");
        if (user.Position != Position.Educator)
            throw new PositionException("The user is not an educator");
        if (await _prsuRepo.IsAlreadyAssigned(UserGuid, SubjGuid))
            throw new UpdateFailedException("The user has this subject assigned already");
        await _prsuRepo.AssignUserToSubjectAsync(UserGuid, SubjGuid);
    }

    public async Task DismissUserFromSubjectAsync(string UserId, string SubjectId)
    {
        Guid UserGuid = Guid.Parse(UserId);
        Guid SubjectGuid = Guid.Parse(SubjectId);
        User? user = await _userRepo.GetUserByGuidAsync(UserGuid) ??
        throw new UserDoesntExistException("No such user");
        Subject? subject = await _subjRepo.GetSubjectByGuidAsync(SubjectGuid) ??
        throw new SubjectDoesntExistException("No such subject");
        if (user.Position != Position.Educator)
            throw new PositionException("The user is not an educator");
        if (await _prsuRepo.IsAlreadyAssigned(UserGuid, SubjectGuid))
            await _prsuRepo.DismissUserFromSubjectAsync(UserGuid, SubjectGuid);
        throw new UpdateFailedException("The user is not assigned to this subject");
    }

    public async Task<List<Subject>> GetSubjectsAssignedAsync(string UserId)
    {
        Guid UserGuid = Guid.Parse(UserId);
        User? user = await _userRepo.GetUserByGuidAsync(UserGuid) ??
        throw new UserDoesntExistException("No such user");
        return await _prsuRepo.GetSubjectsAssignedAsync(UserGuid);
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