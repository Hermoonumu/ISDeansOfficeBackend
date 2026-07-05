namespace DeanInfoSystem.Application.StudentGrades;


public interface IStudentGradeService
{
    public Task GradeAsync(string cardId, uint grade);
}