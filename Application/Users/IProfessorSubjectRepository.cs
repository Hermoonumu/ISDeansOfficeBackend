using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Users;



public interface IProfessorSubjectRepository
{
    public Task AssignUserToSubjectAsync(Guid UserId, Guid SubjectId);
    public Task DismissUserFromSubjectAsync(Guid UserId, Guid SubjectId);
    public Task<List<Subject>> GetSubjectsAssignedAsync(Guid UserId);
    public Task<bool> IsAlreadyAssigned(Guid UserId, Guid SubjectId);
}