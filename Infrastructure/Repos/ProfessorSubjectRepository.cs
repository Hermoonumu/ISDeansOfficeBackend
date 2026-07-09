using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class ProfessorSubjectRepository(SystemDbContext _db) : IProfessorSubjectRepository
{
    public async Task AssignUserToCurriculumAsync(Guid UserId, Guid CurriculumId)
    {
        await _db.EducCurr.AddAsync(new EducatorCurriculum()
        {
            UserId = UserId,
            CurriculumId = CurriculumId
        });
        await _db.SaveChangesAsync();
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
}