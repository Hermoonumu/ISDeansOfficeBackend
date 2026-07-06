namespace DeanInfoSystem.Application.Enrollment;


using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

public class EnrollmentService(IProgramRepository _progRepo,
                            ICurriculaRepository _currRepo,
                            IUserRepository _userRepo,
                            IStudentGradeRepository _sgRepo) : IEnrollmentService
{
    public async Task EnrollStudentIntoProgramAsync(Guid StudentId, Guid ProgramId)
    {
        User? student = await _userRepo.GetUserByGuidAsync(StudentId)
        ?? throw new UserDoesntExistException("No such user");
        EdProgram? edProgram = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");

        if (student.Position != Position.Student)
            throw new PositionException("This user is not a student");

        if (student.ProgramId != null)
            throw new EnrollmentException("This student is already enrolled in a program");

        student.ProgramId = ProgramId;
        await _userRepo.PersistChangesAsync();

        List<Curriculum> curricula = await _currRepo.GetAllCurriculaByProgramAsync(ProgramId);

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