namespace DeanInfoSystem.Application.Enrollment;


using DeanInfoSystem.API.StudentGrades;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.Enrollment;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

public class EnrollmentService(IProgramRepository _progRepo,
                            ICurriculaRepository _currRepo,
                            IUserRepository _userRepo,
                            IStudentGradeRepository _sgRepo) : IEnrollmentService
{
    public async Task EnrollStudentIntoProgramAsync(string StudentId, string ProgramId)
    {
        Guid progId = Guid.Parse(ProgramId);
        User? student = await _userRepo.GetUserByGuidAsync(Guid.Parse(StudentId))
        ?? throw new UserDoesntExistException("No such user");
        EdProgram? edProgram = await _progRepo.GetProgramByGuidAsync(progId)
        ?? throw new ProgramDoesntExistException("No such program");

        if (student.Position != Position.Student)
            throw new PositionException("This user is not a student");

        if (student.ProgramId != null)
            throw new EnrollmentException("This student is already enrolled in a program");

        student.ProgramId = progId;
        await _userRepo.PersistChangesAsync();

        List<Curriculum> curricula = await _currRepo.GetAllCurriculaByProgramAsync(progId);

        foreach (Curriculum curr in curricula)
        {
            StudentGrade sg = new StudentGrade()
            {
                StudentId = student.Id,
                Status = Status.Pending,
                CurriculumId = curr.Id
            };
            await _sgRepo.InstantiateGradeAsync(sg);
        }
    }
}