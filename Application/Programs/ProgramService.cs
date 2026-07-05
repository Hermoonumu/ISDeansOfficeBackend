using DeanInfoSystem.API.StudentGrades;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Users;
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

    public async Task<Guid> AssignSubjectToProgramAsync(string ProgramId,
                                                    AddSubjectToProgramDTO astpDTO)
    {
        EdProgram? Program = await _progRepo.GetProgramByGuidAsync(Guid.Parse(ProgramId))
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

}