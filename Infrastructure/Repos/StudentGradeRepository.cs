using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;




public class StudentGradeRepository(SystemDbContext _db) : IStudentGradeRepository
{
    public async Task<List<StudentGrade>> GetGradesByCurriculumAsync(Guid CurrId)
    {
        return await _db.Grades.Where(g => g.CurriculumId == CurrId).ToListAsync();
    }

    public async Task<List<StudentGrade>> GetGradesByEducatorIdAsync(Guid UserId)
    {
        return await _db.Grades
        .Include(g => g.Student)
        .Include(g => g.Curriculum)
        .ThenInclude(c => c.Subject)
        .Where(grade =>
            _db.EducCurr.Any(ec =>
                ec.UserId == UserId &&
                ec.CurriculumId == grade.CurriculumId))
        .ToListAsync();
    }

    public async Task<StudentGrade?> GetStudentGradeByGuidAsync(Guid guid)
    {
        return await _db.Grades.Where(g => g.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<List<StudentGrade>> GetStudentGradeByStudentIdRangeAsync(List<Guid> UserIds)
    {
        return await _db.Grades.Where(sg => UserIds.Contains((Guid)sg.StudentId)).ToListAsync();
    }

    public async Task<List<StudentGrade>> GetStudentGradeRangeAsync(List<Guid> sgIds)
    {
        return await _db.Grades.Where(sg => sgIds.Contains(sg.Id)).ToListAsync();
    }

    public async Task<List<StudentGrade>> GetStudentGradesAsync(Guid StudentId)
    {
        return await _db.Grades.AsNoTracking().Where(g => g.StudentId == StudentId).ToListAsync();
    }

    public async Task InstantiateGradeAsync(StudentGrade sg)
    {
        await _db.Grades.AddAsync(sg);
    }

    public async Task InstantiateGradesRangeAsync(List<StudentGrade> sgs)
    {
        await _db.Grades.AddRangeAsync(sgs);
    }

    public async Task RemoveGradesRangeAsync(List<Guid> sgIds)
    {
        await _db.Grades.Where(g => sgIds.Contains(g.Id)).ExecuteDeleteAsync();
    }
}