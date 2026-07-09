using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public class StudentGradeService(IStudentGradeRepository _sgRepo,
                                IUserService _usrSvc) : IStudentGradeService
{
    public async Task BulkGradeAsync(List<BulkGradeDTO> bgDTO)
    {
        List<StudentGrade> sgs = [];
        foreach (BulkGradeDTO g in bgDTO)
        {
            sgs.Add(await _sgRepo.GetStudentGradeByGuidAsync(g.GradeCardId) ??
            throw new GradeDoesntExistException($"No such grade (ID: {g.GradeCardId})"));
        }
        for (int i = 0; i < sgs.Count(); i++)
        {
            sgs[i].Grade = (int)bgDTO[i].Grade;
        }
        await _sgRepo.PersistChangesAsync();
    }

    public async Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId)
    {
        User? user = await _usrSvc.GetUserByGuidAsync(StudentId) ??
        throw new UserDoesntExistException("No such user");

        if (user.Position != Position.Student)
            throw new PositionException("The user is not a student to get their grade");

        return await _sgRepo.GetStudentGradesAsync(StudentId);
    }

    public async Task GradeAsync(Guid cardId, uint grade)
    {
        StudentGrade? sg = await _sgRepo.GetStudentGradeByGuidAsync(cardId)
        ?? throw new GradeDoesntExistException("No such grade");

        sg.Grade = (int)grade;

        if (grade < 60) sg.Status = Status.Failed;
        else
        {
            sg.Status = Status.Passed;
            sg.PassedDate = DateTime.UtcNow;
        }

        await _sgRepo.PersistChangesAsync();
    }
}