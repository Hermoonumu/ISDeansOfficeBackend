using System.Transactions;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public class StudentGradeService(IStudentGradeRepository _sgRepo,
                                IUserService _usrSvc,
                                IEducatorCurriculumRepository _edcuRepo,
                                IUnitOfWork _uow) : IStudentGradeService
{
    public async Task BulkGradeAsync(User user, List<BulkGradeDTO> bgDTO)
    {

        List<StudentGrade> sgs = await _sgRepo.GetStudentGradeRangeAsync([.. bgDTO.Select(e => e.GradeCardId)]);
        if (user.Position != Position.Dean)
        {
            if ((await _edcuRepo.IsAlreadyAssignedRangeAsync(user.Id, [.. sgs.Select(e => (Guid)e.CurriculumId)])).Any(e => e == false))
            {
                throw new PositionException("The user is not authorized to grade some of the curricula");
            }
        }
        Dictionary<Guid, StudentGrade> sgsDict = [];
        Dictionary<Guid, uint> dtoDict = [];
        sgs.ForEach(g => sgsDict.Add(g.Id, g));

        dtoDict = bgDTO
        .GroupBy(x => x.GradeCardId)
        .ToDictionary(g => g.Key, g => g.Last().Grade);

        using var tr = await _uow.BeginTransactionAsync();
        foreach (Guid Id in sgsDict.Keys)
        {
            sgsDict[Id].Grade = (int)dtoDict[Id];
            if (dtoDict[Id] >= 60)
            {
                sgsDict[Id].GradingDate = DateTime.UtcNow;
            }
            else
            {
                sgsDict[Id].Status = Status.Failed;
            }
            sgsDict[Id].Status = Status.Passed;
            sgsDict[Id].GradedById = user.Id;
        }
        await _uow.SaveChangesAsync();
        await _uow.CommitTransactionAsync();
    }

    public async Task ConfirmFailingGrade(Guid GradeId)
    {
        StudentGrade? grade = await _sgRepo.GetStudentGradeByGuidAsync(GradeId)
        ?? throw new GradeDoesntExistException("No such grade");

        if (grade.Status != Status.Failed)
            throw new PositionException("This grade is not failing for it to be deemed failed");

        using var tr = await _uow.BeginTransactionAsync();
        grade.ConfirmFailure = true;
        await _uow.SaveChangesAsync();
        await _uow.CommitTransactionAsync();
    }

    public async Task<List<StudentGrade>> GetGradesByCurriculumAsync(User user, Guid CurrId)
    {
        if (user.Position != Position.Dean)
        {
            if (!await _edcuRepo.IsAlreadyAssigned(user.Id, CurrId))
            {
                throw new PositionException("The user is not authorized to access the grades of this curriculum");
            }
        }
        return await _sgRepo.GetGradesByCurriculumAsync(CurrId);
    }

    public async Task<List<StudentGrade>> GetGradesByEducatorIdAsync(Guid UserId)
    {
        return await _sgRepo.GetGradesByEducatorIdAsync(UserId);
    }

    public async Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId)
    {
        User? user = await _usrSvc.GetUserByGuidAsync(StudentId) ??
        throw new UserDoesntExistException("No such user");

        if (user.Position != Position.Student)
            throw new PositionException("The user is not a student to get their grade");

        var grades = await _sgRepo.GetStudentGradesAsync(StudentId);
        return grades;
    }

    public async Task<List<StudentGradeDTO>> GetStudentGradesDTOAsync(Guid StudentId)
    {
        User? user = await _usrSvc.GetUserByGuidAsync(StudentId) ??
        throw new UserDoesntExistException("No such user");

        if (user.Position != Position.Student)
            throw new PositionException("The user is not a student to get their grade");

        var grades = await _sgRepo.GetStudentGradesDTOAsync(StudentId);
        return grades;
    }

    public async Task<List<StudentGradeDTO>> GetStudentRecentGradesAsync(Guid userId, int amount)
    {
        User? user = await _usrSvc.GetUserByGuidAsync(userId) ??
        throw new UserDoesntExistException("No such user");

        if (user.Position != Position.Student)
            throw new PositionException("The user is not a student to get their grade");

        return await _sgRepo.GetStudentRecentGradesAsync(userId, amount);
    }

    public async Task<List<UngradedStudentsDTO>> GetUngradedStudentsAsync(Guid EducatorId)
    {
        return await _sgRepo.GetUngradedStudentsAsync(EducatorId);
    }

    public async Task GradeAsync(User user, Guid cardId, uint grade)
    {
        StudentGrade? sg = await _sgRepo.GetStudentGradeByGuidAsync(cardId)
        ?? throw new GradeDoesntExistException("No such grade");

        if (user.Position != Position.Dean)
        {
            if (!await _edcuRepo.IsAlreadyAssigned(user.Id, (Guid)sg.CurriculumId!))
            {
                throw new PositionException("The user is not authorized to grade this curriculum");
            }
        }

        sg.Grade = (int)grade;
        using var tr = await _uow.BeginTransactionAsync();
        sg.Status = Status.Passed;
        sg.GradingDate = DateTime.UtcNow;
        sg.GradedById = user.Id;
        sg.Status = grade < 60 ? Status.Failed : Status.Passed;
        await _uow.SaveChangesAsync();
        await _uow.CommitTransactionAsync();
    }
}