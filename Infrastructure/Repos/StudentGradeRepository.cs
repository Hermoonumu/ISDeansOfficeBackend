using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;




public class StudentGradeRepository(SystemDbContext _db) : IStudentGradeRepository
{
    public async Task<StudentGrade?> GetStudentGradeByGuidAsync(Guid guid)
    {
        return await _db.Grades.Where(g => g.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId)
    {
        return await _db.Grades.Where(g => g.StudentId == StudentId).ToListAsync();
    }

    public async Task InstantiateGradeAsync(StudentGrade sg)
    {
        await _db.Grades.AddAsync(sg);
        await _db.SaveChangesAsync();
    }

    public async Task InstantiateGradesRangeAsync(List<StudentGrade> sgs)
    {
        await _db.Grades.AddRangeAsync(sgs);
        await _db.SaveChangesAsync();
    }

    public async Task PersistChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}