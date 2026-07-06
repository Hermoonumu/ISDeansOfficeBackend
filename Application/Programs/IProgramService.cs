using DeanInfoSystem.Application.DTO;

namespace DeanInfoSystem.Application.Programs;



public interface IProgramService
{
    public Task AddProgramAsync(NewProgramDTO npDTO);
    public Task<Guid> AssignSubjectToProgramAsync(Guid ProgramId, AddSubjectToProgramDTO astpDTO);
}