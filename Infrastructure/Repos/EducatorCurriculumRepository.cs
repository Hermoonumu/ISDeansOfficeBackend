using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class EducatorCurriculumRepository(SystemDbContext _db) : IEducatorCurriculumRepository
{
    public async Task AssignUserToCurriculumAsync(Guid UserId, Guid CurriculumId)
    {
        await _db.EducCurr.AddAsync(new EducatorCurriculum()
        {
            UserId = UserId,
            CurriculumId = CurriculumId
        });
    }

    public async Task DismissUserFromCurriculumAsync(Guid UserId, Guid CurriculumId)
    {
        await _db.EducCurr.Where(e => e.CurriculumId == CurriculumId && e.UserId == UserId).ExecuteDeleteAsync();
    }

    public async Task<List<Curriculum?>> GetCurriculaAssignedAsync(Guid UserId)
    {
        return [.. (await _db.EducCurr
                            .Include(e => e.Curriculum)
                            .Where(u => u.UserId == UserId)
                            .ToListAsync())
                            .Select(e => e.Curriculum)];

    }

    public async Task<bool> IsAlreadyAssigned(Guid UserId, Guid CurriculumId)
    {
        EducatorCurriculum? test = await _db.EducCurr
                                            .Where(e => e.UserId == UserId
                                            && e.CurriculumId == CurriculumId)
                                            .FirstOrDefaultAsync();
        if (test is null) return false;
        return true;
    }

    public async Task<List<bool>> IsAlreadyAssignedRangeAsync(Guid UserId, List<Guid> CurriculumIds)
    {
        var assignedIds = await _db.EducCurr
                                    .Where(e => e.UserId == UserId && CurriculumIds.Contains(e.CurriculumId))
                                    .Select(e => e.CurriculumId)
                                    .ToListAsync();
        return CurriculumIds.Select(id => assignedIds.Contains(id)).ToList();
    }
}