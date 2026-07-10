using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;



public interface IProgramService
{
    public Task AddProgramAsync(NewProgramDTO npDTO);
    public Task<Guid> AssignSubjectToProgramAsync(Guid ProgramId, AddSubjectToProgramDTO astpDTO);
    public Task RemoveCurriculumFromProgramAsync(Guid CurrId);
    public Task ChangeProgramStatusAsync(ProgramStatus status, Guid ProgramId);
    public Task<List<EdProgram>> GetProgramsPageAsync(int page, int take);
    public Task RemoveProgramAsync(Guid ProgramId);
}