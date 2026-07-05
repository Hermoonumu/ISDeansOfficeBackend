using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.StudentGrades;


public interface IStudentGradeRepository
{
    public Task InstantiateGradeAsync(StudentGrade sg);
    public Task<StudentGrade?> GetStudentGradeByGuidAsync(Guid guid);
    public Task PersistChangesAsync();
}