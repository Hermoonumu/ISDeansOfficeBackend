using DeanInfoSystem.Application.Analytics;
using DeanInfoSystem.Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class AnalyticsRepository(SystemDbContext _db) : IAnalyticsRepository
{
    public async Task<List<GradeBucketsDTO>> GetProgramGradeBucketsAsync(Guid ProgramId)
    {
        return await _db.Grades
                        .Where(g => g.Curriculum.EdProgramId == ProgramId)
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
}