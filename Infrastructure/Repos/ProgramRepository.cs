using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;




public class ProgramRepository(SystemDbContext _db) : IProgramRepository
{
    public async Task AddProgramAsync(EdProgram program)
    {
        await _db.Programs.AddAsync(program);
        await _db.SaveChangesAsync();
    }

    public async Task<EdProgram?> GetProgramByGuidAsync(Guid guid)
    {
        return await _db.Programs.Where(p => p.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<List<EdProgram>> GetProgramsPageAsync(int page, int take)
    {
        return await _db.Programs.Skip(page * 10).Take(take).ToListAsync();
    }

    public async Task PersistChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task RemoveProgramAsync(Guid ProgramId)
    {
        await _db.Programs.Where(p => p.Id == ProgramId).ExecuteDeleteAsync();
    }
}