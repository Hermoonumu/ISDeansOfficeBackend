using DeanInfoSystem.Domain;

namespace DeanInfoSystem.API.StudentGrades;


public interface IStudentGradeRepository
{
    public Task InstantiateGradeAsync(StudentGrade sg);
}