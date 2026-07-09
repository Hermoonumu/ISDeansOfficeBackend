using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Subjects;

public interface ISubjectRepository
{
    public Task AddSubjectAsync(Subject subject);
    public Task<bool> IsSubjectNameTaken(string name);
    public Task<Subject?> GetSubjectByGuidAsync(Guid guid);
    public Task RemoveSubjectAsync(Guid guid);
    public Task PersistChangesAsync();
    public Task<List<Subject>> GetAllSubjectsPageAsync(int page, int take);
    public Task<List<Subject>> GetAllSubjectsAsync();
}