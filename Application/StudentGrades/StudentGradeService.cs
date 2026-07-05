using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public class StudentGradeService(IStudentGradeRepository _sgRepo) : IStudentGradeService
{
    public async Task GradeAsync(string cardId, uint grade)
    {
        Guid guid = Guid.Parse(cardId);
        StudentGrade? sg = await _sgRepo.GetStudentGradeByGuidAsync(guid)
        ?? throw new GradeDoesntExistException("No such grade");

        if (sg.Status == Status.Passed) throw new UpdateFailedException("The exam has been passed");

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