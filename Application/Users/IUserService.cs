using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task AddUserAsync(NewUserDTO nuDTO);
    public Task PatchUserAsync(Guid guid, JsonPatchDocument<UserUpdateDTO> Patch);
    public Task RemoveUserAsync(Guid guid);
    public Task AssignProfToSubjectAsync(Guid SubjId, Guid UserId);
    public Task DismissUserFromSubjectAsync(Guid UserId, Guid SubjectId);
    public Task<List<Subject>> GetSubjectsAssignedAsync(Guid UserId);
}