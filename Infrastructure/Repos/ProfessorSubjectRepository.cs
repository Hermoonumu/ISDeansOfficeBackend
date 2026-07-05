using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeanInfoSystem.Infrastructure.Repos;


public class ProfessorSubjectRepository(SystemDbContext _db) : IProfessorSubjectRepository
{
    public async Task AssignUserToSubjectAsync(Guid UserId, Guid SubjectId)
    {
        await _db.UserSubj.AddAsync(new ProfessorSubject()
        {
            UserId = UserId,
            SubjectId = SubjectId
        });
        await _db.SaveChangesAsync();
    }

    public async Task DismissUserFromSubjectAsync(Guid UserId, Guid SubjectId)
    {
        await _db.UserSubj.Where(e => e.SubjectId == SubjectId && e.UserId == UserId).ExecuteDeleteAsync();
    }

    public async Task<List<Subject>> GetSubjectsAssignedAsync(Guid UserId)
    {
        return [.. (await _db.UserSubj
                            .Include(e => e.Subject)
                            .Where(u => u.UserId == UserId)
                            .ToListAsync())
                            .Select(e => e.Subject)];

    }

    public async Task<bool> IsAlreadyAssigned(Guid UserId, Guid SubjectId)
    {
        ProfessorSubject? test = await _db.UserSubj
                                            .Where(e => e.UserId == UserId
                                            && e.SubjectId == SubjectId)
                                            .FirstOrDefaultAsync();
        if (test is null) return false;
        return true;
    }
}