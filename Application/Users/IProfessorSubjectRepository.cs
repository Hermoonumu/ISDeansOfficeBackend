using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Users;



public interface IProfessorSubjectRepository
{
    public Task AssignUserToCurriculumAsync(Guid UserId, Guid CurriculumId);
    public Task DismissUserFromCurriculumAsync(Guid UserId, Guid CurriculumId);
    public Task<List<Curriculum?>> GetCurriculaAssignedAsync(Guid UserId);
    public Task<bool> IsAlreadyAssigned(Guid UserId, Guid CurriculumId);
}