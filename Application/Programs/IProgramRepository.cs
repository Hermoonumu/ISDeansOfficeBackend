using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;




public interface IProgramRepository
{
    public Task AddProgramAsync(EdProgram program);
    public Task<EdProgram?> GetProgramByGuidAsync(Guid guid);
    public Task<List<EdProgram>> GetProgramsPageAsync(int page, int take);
    public Task RemoveProgramAsync(Guid ProgramId);
    public Task PersistChangesAsync();
}