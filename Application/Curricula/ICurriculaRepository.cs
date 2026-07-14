using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Curricula;




public interface ICurriculaRepository
{
    public Task AddCurriculumAsync(Curriculum curriculum);
    public Task<List<Curriculum>> GetAllCurriculaByProgramAsync(Guid programId);
    public Task<Curriculum?> GetCurriculumByIdAsync(Guid CurriculumId);
    public Task<Curriculum?> GetCurriculumBySubjectProgramAsync(Guid SubjId, Guid ProgramId);
    public Task<List<Curriculum>> GetCurriculaAssignedAsync(Guid UserId);
    public Task RemoveCurriculumAsync(Guid Id);
}