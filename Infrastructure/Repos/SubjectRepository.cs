using DeanInfoSystem.Application.Subjects;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;



public class SubjectRepository(SystemDbContext _db) : ISubjectRepository
{
    public async Task AddSubjectAsync(Subject subject)
    {
        await _db.Subjects.AddAsync(subject);
        await _db.SaveChangesAsync();
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

    public async Task PersistChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task RemoveSubjectAsync(Guid guid)
    {
        await _db.Subjects.Where(s => s.Id == guid).ExecuteDeleteAsync();
    }
}