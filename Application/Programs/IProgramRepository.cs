using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;




public interface IProgramRepository
{
    public Task AddProgramAsync(EdProgram program);
    public Task<EdProgram?> GetProgramByGuidAsync(Guid guid);
}