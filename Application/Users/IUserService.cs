using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Users;


public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task AddUserAsync(NewUserDTO nuDTO);
    public Task PatchUserAsync(string guid, JsonPatchDocument<UserUpdateDTO> Patch);
    public Task RemoveUserAsync(string guid);
    public Task AssignProfToSubjectAsync(string SubjId, string UserId);
    public Task DismissUserFromSubjectAsync(string UserId, string SubjectId);
    public Task<List<Subject>> GetSubjectsAssignedAsync(string UserId);
}