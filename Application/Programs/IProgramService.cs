using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;



public interface IProgramService
{
    public Task AddProgramAsync(NewProgramDTO npDTO);
    public Task<Guid> AssignSubjectToProgramAsync(Guid ProgramId, AddSubjectToProgramDTO astpDTO);
    public Task<List<Subject>> GetAllSubjectsAsync(int page = 0, int take = 10);
}