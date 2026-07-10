using System.Transactions;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Enrollment;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Programs;




public class ProgramService(IProgramRepository _progRepo,
                            IDepartmentRepository _deptRepo,
                            ICurriculaRepository _currRepo,
                            IEnrollmentService _enrSvc,
                            IUnitOfWork _uow) : IProgramService
{
    public async Task AddProgramAsync(NewProgramDTO npDTO)
    {
        Department? department = await _deptRepo.GetDepartmentByGuidAsync(Guid.Parse(npDTO.DepartmentId)) ??
        throw new DepartmentDoesntExistException("No such department");
        var tr = await _uow.BeginTransactionAsync();
        await _progRepo.AddProgramAsync(
            new EdProgram()
            {
                ProgramName = npDTO.ProgramName,
                ProgramCode = npDTO.ProgramCode,
                DepartmentId = department.Id,
                ProgramStatus = ProgramStatus.Drafted
            }
        );
        await _uow.SaveChangesAsync();
        tr.Commit();
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
        using var tr = await _uow.BeginTransactionAsync();
        await _currRepo.AddCurriculumAsync(curriculum);
        await _uow.SaveChangesAsync();
        await _enrSvc.UpdateStudentGradesOnNewCurriculumAsync(curriculum.Id);
        await _uow.SaveChangesAsync();
        tr.Commit();
        return curriculum.Id;
    }

    public async Task ChangeProgramStatusAsync(ProgramStatus status, Guid ProgramId)
    {
        EdProgram? program = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");
        using var tr = await _uow.BeginTransactionAsync();
        program.ProgramStatus = status;
        await _uow.SaveChangesAsync();
        tr.Commit();
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

    public async Task RemoveCurriculumFromProgramAsync(Guid CurrId)
    {
        await _currRepo.RemoveCurriculumAsync(CurrId);
    }

    public async Task<List<Curriculum>> GetProgramCurriculaAsync(Guid? ProgramId)
    {
        if (ProgramId is null) throw new PositionException("The student isn't enrolled in any program");
        return await _currRepo.GetAllCurriculaByProgramAsync((Guid)ProgramId);
    }
}