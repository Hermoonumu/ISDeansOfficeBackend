namespace DeanInfoSystem.Application.StudentGrades;


public interface IStudentGradeService
{
    public Task GradeAsync(Guid cardId, uint grade);
}