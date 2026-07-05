using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Curricula;




public interface ICurriculaRepository
{
    public Task AddCurriculumAsync(Curriculum curriculum);
    public Task<List<Curriculum>> GetAllCurriculaByProgramAsync(Guid programId);
}