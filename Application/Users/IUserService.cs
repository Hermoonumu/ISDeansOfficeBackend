using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task AddUserAsync(NewUserDTO nuDTO);
    public Task<User?> GetUserByGuidAsync(Guid UserId);
    public Task PatchUserAsync(Guid guid, JsonPatchDocument<UserUpdateDTO> Patch);
    public Task RemoveUserAsync(Guid guid);
    public Task AssignProfToSubjectAsync(Guid UserId, Guid SubjId);
    public Task DismissUserFromSubjectAsync(Guid UserId, Guid SubjectId);
    public Task<List<Subject>> GetSubjectsAssignedAsync(Guid UserId);
    public Task<List<UserDTO>> GetAllUsersPageAsync(int page, int take);
    public Task<List<UserDTO>> GetAllUsersByPositionPageAsync(Position pos, int page, int take);
}