using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DeanInfoSystem.Application.Programs;



public interface IProgramService
{
    public Task AddProgramAsync(NewProgramDTO npDTO);
    public Task<Guid> AssignSubjectToProgramAsync(Guid ProgramId, AddSubjectToProgramDTO astpDTO);
    public Task RemoveCurriculumFromProgramAsync(Guid CurrId);
    public Task RemoveSubjectFromProgramAsync(Guid SubjId, Guid ProgramId);
    public Task ChangeProgramStatusAsync(ProgramStatus status, Guid ProgramId);
    public Task<List<EdProgram>> GetProgramsPageAsync(int page, int take);
    public Task RemoveProgramAsync(Guid ProgramId);
    public Task<List<Curriculum?>> GetEducatorAssignedCurricula(Guid UserId);
    public Task<List<Curriculum>> GetProgramCurriculaAsync(Guid? ProgramId, Semester semester = Semester.FirstYearFirstSemester);
    public Task PatchProgramAsync(Guid ProgramId, JsonPatchDocument<ProgramPatchDTO> ppDTO);
    public Task<List<EdProgram>> GetAllProgramsAsync();
    public Task<List<Curriculum>> GetAllCurriculaAsync();
}