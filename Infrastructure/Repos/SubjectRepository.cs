using DeanInfoSystem.Application.Subjects;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;



public class SubjectRepository(SystemDbContext _db) : ISubjectRepository
{
    public async Task AddSubjectAsync(Subject subject)
    {
        await _db.Subjects.AddAsync(subject);
    }

    public async Task<List<Subject>> GetAllSubjectsAsync()
    {
        return await _db.Subjects.ToListAsync();
    }

    public async Task<List<Subject>> GetAllSubjectsPageAsync(int page, int take)
    {
        return await _db.Subjects.Skip(page * take)
                                    .Take(take)
                                    .ToListAsync();
    }

    public async Task<Subject?> GetSubjectByGuidAsync(Guid guid)
    {
        return await _db.Subjects.Where(s => s.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<bool> IsSubjectNameTaken(string name)
    {
        Subject? subject = await _db.Subjects.Where(s => s.SubjectName == name).FirstOrDefaultAsync();
        if (subject is null) return false;
        return true;
    }

    public async Task RemoveSubjectAsync(Guid guid)
    {
        await _db.Subjects.Where(s => s.Id == guid).ExecuteDeleteAsync();
    }
}