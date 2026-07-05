using DeanInfoSystem.API.StudentGrades;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Infrastructure.Repos;




public class StudentGradeRepository(SystemDbContext _db) : IStudentGradeRepository
{
    public async Task InstantiateGradeAsync(StudentGrade sg)
    {
        await _db.Grades.AddAsync(sg);
        await _db.SaveChangesAsync();
    }
}