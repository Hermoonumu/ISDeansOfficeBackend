using DeanInfoSystem.Application.DTO;
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
        return await _db.Grades.Include(g => g.GradedBy)
                                            .Include(g => g.Curriculum)
                                            .ThenInclude(c => c.Subject)
                                            .Where(g => g.StudentId == StudentId)
                                            .AsNoTracking()
                                            .ToListAsync();
    }


    public async Task<List<StudentGradeDTO>> GetStudentGradesDTOAsync(Guid StudentId)
    {
        return await _db.Grades.Include(g => g.GradedBy)
                    .Include(g => g.Curriculum)
                    .ThenInclude(c => c.Subject)
                    .Where(g => g.StudentId == StudentId)
                    .AsNoTracking()
                    .Select(g => new StudentGradeDTO
                    {
                        Id = g.Id,
                        StudentId = StudentId,
                        Grade = g.Grade,
                        CurriculumId = g.CurriculumId,
                        Curriculum = g.Curriculum,
                        GradingDate = g.GradingDate,
                        ProgramName = g.Curriculum.EdProgram.ProgramName,
                        SubjectName = g.Curriculum.Subject.SubjectName,
                        Status = g.Status,
                        GradedById = g.GradedById,
                        GradedBy = new UserDTO()
                        {
                            Id = g.GradedBy.Id,
                            FirstName = g.GradedBy.FirstName,
                            LastName = g.GradedBy.LastName,
                            Position = g.GradedBy.Position,
                            Username = g.GradedBy.Username
                        }
                    })
                    .ToListAsync();
    }
    public async Task<List<StudentGradeDTO>> GetStudentRecentGradesAsync(Guid userId, int amount)
    {
        return await _db.Grades
            .Where(g => g.StudentId == userId && g.Grade != null)
            .OrderBy(g => g.GradingDate)
            .Take(amount)
            .Select(g => new StudentGradeDTO
            {
                Id = g.Id,
                StudentId = userId,
                Grade = g.Grade,
                CurriculumId = g.CurriculumId,
                GradingDate = g.GradingDate,
                ProgramName = g.Curriculum.EdProgram.ProgramName,
                SubjectName = g.Curriculum.Subject.SubjectName,
                Status = g.Status,
                GradedById = g.GradedById,
                GradedBy = new UserDTO()
                {
                    Id = g.GradedBy.Id,
                    FirstName = g.GradedBy.FirstName,
                    LastName = g.GradedBy.LastName,
                    Position = g.GradedBy.Position,
                    Username = g.GradedBy.Username
                }
            })
            .ToListAsync();
    }

    public async Task<List<UngradedStudentsDTO>> GetUngradedStudentsAsync(Guid EducatorId)
    {
        return await _db.Grades.Where(g => g.Status != Status.Passed && !g.ConfirmFailure)
                                .Select(g => new UngradedStudentsDTO()
                                {
                                    GradeId = g.Id.ToString(),
                                    StudentId = g.StudentId.ToString(),
                                    FullName = g.Student.FirstName + " " + g.Student.LastName,
                                    CurriculumId = g.CurriculumId.ToString(),
                                    SubjectName = g.Curriculum.Subject.SubjectName,
                                    Status = g.Status,
                                    Grade = g.Grade
                                })
                                .ToListAsync();
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