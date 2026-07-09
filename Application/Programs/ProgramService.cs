using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;




public class ProgramService(IProgramRepository _progRepo,
                            IDepartmentRepository _deptRepo,
                            ICurriculaRepository _currRepo) : IProgramService
{
    public async Task AddProgramAsync(NewProgramDTO npDTO)
    {
        Department? department = await _deptRepo.GetDepartmentByGuidAsync(Guid.Parse(npDTO.DepartmentId)) ??
        throw new DepartmentDoesntExistException("No such department");
        await _progRepo.AddProgramAsync(
            new EdProgram()
            {
                ProgramName = npDTO.ProgramName,
                ProgramCode = npDTO.ProgramCode,
                DepartmentId = department.Id,
                ProgramStatus = ProgramStatus.Drafted
            }
        );
    }

    public async Task<Guid> AssignSubjectToProgramAsync(Guid ProgramId,
                                                    AddSubjectToProgramDTO astpDTO)
    {
        EdProgram? Program = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");

        Curriculum curriculum = new()
        {
            EdProgramId = Program.Id,
            SubjectId = astpDTO.SubjectId,
            Semester = astpDTO.Semester,
            LectureHours = astpDTO.LectureHours,
            PracticeHours = astpDTO.PracticeHours,
            LabHours = astpDTO.LabHours,
            CourseWorkHours = astpDTO.CourseWorkHours,
            AssessmentType = astpDTO.AssessmentType
        };

        await _currRepo.AddCurriculumAsync(curriculum);
        return curriculum.Id;
    }

    public async Task ChangeProgramStatusAsync(ProgramStatus status, Guid ProgramId)
    {
        EdProgram? program = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");
        program.ProgramStatus = status;
        await _progRepo.PersistChangesAsync();
    }

    public async Task<List<EdProgram>> GetProgramsPageAsync(int page, int take)
    {
        return await _progRepo.GetProgramsPageAsync(page, take);
    }

    public async Task RemoveProgramAsync(Guid ProgramId)
    {
        EdProgram edProgram = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");
        await _progRepo.RemoveProgramAsync(ProgramId);
    }

    public Task RemoveSubjectFromProgramAsync(Guid CurrId)
    {
        throw new NotImplementedException();
    }
}