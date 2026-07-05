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
}