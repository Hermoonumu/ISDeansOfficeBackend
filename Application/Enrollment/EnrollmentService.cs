namespace DeanInfoSystem.Application.Enrollment;

using System.Transactions;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

public class EnrollmentService(IProgramRepository _progRepo,
                            ICurriculaRepository _currRepo,
                            IUserRepository _userRepo,
                            IStudentGradeRepository _sgRepo,
                            IUnitOfWork _uow) : IEnrollmentService
{
    public async Task EnrollStudentIntoProgramAsync(Guid StudentId, Guid ProgramId)
    {
        User? student = await _userRepo.GetUserByGuidAsync(StudentId)
        ?? throw new UserDoesntExistException("No such user");
        EdProgram? edProgram = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");

        if (edProgram.ProgramStatus != ProgramStatus.Approved)
            throw new EnrollmentException("Cannot enroll in a program that is not active.");

        if (student.Position != Position.Student)
            throw new PositionException("This user is not a student");

        if (student.ProgramId != null)
            throw new EnrollmentException("This student is already enrolled in a program");

        student.ProgramId = ProgramId;
        List<Curriculum> curricula = await _currRepo.GetAllCurriculaByProgramAsync(ProgramId);
        List<StudentGrade> sgs = [];
        foreach (Curriculum curr in curricula)
        {
            sgs.Add(new StudentGrade()
            {
                StudentId = student.Id,
                Status = Status.Pending,
                CurriculumId = curr.Id
            });
        }
        using var tr = await _uow.BeginTransactionAsync();
        await _sgRepo.InstantiateGradesRangeAsync(sgs);
        await _uow.SaveChangesAsync();
        tr.Commit();
    }

    public async Task UnenrollStudentAsync(Guid StudentId)
    {
        User? user = await _userRepo.GetUserByGuidAsync(StudentId)
        ?? throw new UserDoesntExistException("No such user");

        if (user.Position != Position.Student)
            throw new UpdateFailedException("Can't unenroll anything but student");
        List<Guid> sgIds = [.. (await _sgRepo.GetStudentGradesAsync(user.Id)).Select(e => e.Id)];

        using var tr = await _uow.BeginTransactionAsync();
        await _sgRepo.RemoveGradesRangeAsync(sgIds);
        user.ProgramId = null;
        await _uow.SaveChangesAsync();
        tr.Commit();
    }

    public async Task UpdateStudentGradesOnNewCurriculumAsync(Guid NewCurriculumId)
    {
        Curriculum curr = await _currRepo.GetCurriculumByIdAsync(NewCurriculumId);
        List<User> students = await _userRepo.GetAllUsersInProgramAsync((Guid)curr.EdProgramId);

        List<StudentGrade> sgs = [];
        foreach (User u in students)
        {
            sgs.Add(new StudentGrade()
            {
                StudentId = u.Id,
                Status = Status.Pending,
                CurriculumId = NewCurriculumId,
            });
        }

        if (_uow.IsTransaction())
        {
            using var tr = await _uow.BeginTransactionAsync();
            await _sgRepo.InstantiateGradesRangeAsync(sgs);
            await _uow.SaveChangesAsync();
            tr.Commit();
        }
        else
        {
            await _sgRepo.InstantiateGradesRangeAsync(sgs);
        }

    }
}