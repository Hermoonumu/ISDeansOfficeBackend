using DeanInfoSystem.Application.Curricula;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;



public class CurriculaRepository(SystemDbContext _db) : ICurriculaRepository
{
    public async Task AddCurriculumAsync(Curriculum curriculum)
    {
        await _db.Curricula.AddAsync(curriculum);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Curriculum>> GetAllCurriculaByProgramAsync(Guid programId)
    {
        return await _db.Curricula.Where(c => c.EdProgramId == programId).ToListAsync();
    }

    public async Task<Curriculum?> GetCurriculumByIdAsync(Guid CurriculumId)
    {
        return await _db.Curricula.Where(c => c.Id == CurriculumId).FirstOrDefaultAsync();
    }

    public async Task<List<Curriculum?>> GetCurriculaAssignedAsync(Guid UserId)
    {
        return await _db.EducCurr.Include(ec => ec.Curriculum)
                                .Where(ec => ec.UserId == UserId)
                                .Select(ec => ec.Curriculum)
                                .ToListAsync();
    }

    public async Task RemoveCurriculumAsync(Guid Id)
    {
        await _db.Curricula.Where(c => c.Id == Id).ExecuteDeleteAsync();
    }
}