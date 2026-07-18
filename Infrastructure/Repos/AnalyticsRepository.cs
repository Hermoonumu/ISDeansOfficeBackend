using DeanInfoSystem.Application.Analytics;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class AnalyticsRepository(SystemDbContext _db) : IAnalyticsRepository
{
    public async Task<int> GetEducatorCountAsync()
    {
        return await _db.Users.Where(u => u.Position == Position.Educator).CountAsync();
    }

    public async Task<List<StudentDeanDashDTO>> GetEndangeredStudentsAsync()
    {
        return await _db.Users
                // Filter: Must have at least one finalized grade below 60
                .Where(u => _db.Grades.Any(g => g.StudentId == u.Id && g.Grade < 60))
                .Select(u => new StudentDeanDashDTO
                {
                    // EF Core translates this directly into a SQL string concatenation
                    FullName = u.FirstName + " " + u.LastName,

                    // EF Core handles the joins automatically based on your navigation properties
                    ProgramName = u.Program.ProgramName,
                    DepartmentName = u.Program.Department.DepartmentName,

                    // Calculate their overall average (SQL AVG)
                    AvgGrade = _db.Grades
                        .Where(g => g.StudentId == u.Id)
                        .Average(g => (double?)g.Grade) ?? 0
                })
                .ToListAsync();
    }

    public async Task<int> GetPendingGradeCountAsync()
    {
        return await _db.Grades.Where(g => g.Status == Status.Pending).CountAsync();
    }

    public async Task<int> GetProgramCountAsync()
    {
        return await _db.Programs.CountAsync();
    }

    public async Task<List<GradeBucketsDTO>> GetProgramGradeBucketsAsync(Guid ProgramId)
    {
        return await _db.Grades
                        .Where(g => g.Curriculum.EdProgramId == ProgramId && g.Status != Domain.Status.Pending)
                        .GroupBy(s =>
                            s.Grade < 60 ? Bin.Fail :
                            s.Grade <= 63 ? Bin.Sufficient :
                            s.Grade <= 74 ? Bin.Satisfactory :
                            s.Grade <= 81 ? Bin.Good :
                            s.Grade <= 89 ? Bin.VeryGood :
                            Bin.Excellent
                        )
                        .Select(group => new GradeBucketsDTO
                        {
                            Bin = group.Key,
                            Count = group.Count()
                        })
                        .ToListAsync();
    }

    public async Task<List<StudentDeanDashDTO>> GetScholarshipQualifyingAsync()
    {
        return await _db.Users
        .Where(u => _db.Grades.Any(g => g.StudentId == u.Id))
        .Where(u => _db.Grades
            .Where(g => g.StudentId == u.Id)
            .All(g => g.Grade == null || g.Grade >= 60))
        .Select(u => new
        {
            Student = u,
            Average = _db.Grades
                .Where(g => g.StudentId == u.Id)
                .Average(g => (double?)g.Grade) ?? 0
        })
        .OrderByDescending(x => x.Average)
        .Take(10)
        .Select(x => new StudentDeanDashDTO
        {
            FullName = x.Student.FirstName + " " + x.Student.LastName,
            ProgramName = x.Student.Program.ProgramName,
            DepartmentName = x.Student.Program.Department.DepartmentName,
            AvgGrade = x.Average
        })
        .ToListAsync();
    }

    public async Task<int> GetStudentCountAsync()
    {
        return await _db.Users.Where(u => u.Position == Position.Student).CountAsync();
    }

    public async Task<PerformanceDTO> GetStudentsPerformanceAsync(Guid UserId)
    {
        var performance = await _db.Grades
            .Where(g => g.StudentId == UserId)
            .GroupBy(g => g.StudentId)
            .Select(g => new PerformanceDTO
            {
                AvgGrade = g.Average(x => (double?)x.Grade) ?? 0,
                Passed = g.Count(x => x.Status == Status.Passed),
                Failed = g.Count(x => x.Status == Status.Failed),
                All = g.Count(),
                Ungraded = g.Count(x => x.Grade == null)
            })
            .FirstOrDefaultAsync();
        return performance ?? new PerformanceDTO();
    }

    public async Task<List<Curriculum>> MissingEducatorCurriculaAsync()
    {
        return await _db.Curricula
            .Include(c => c.Subject)
            .Include(c => c.EdProgram)
            .Where(c => !_db.EducCurr.Any(ec => ec.CurriculumId == c.Id))
            .ToListAsync();
    }
}