using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Subjects;


public interface ISubjectService
{
    public Task<Guid> AddSubjectAsync(NewSubjectDTO nsDTO);
    public Task ChangeSubjectNameAsync(Guid Id, string name);
    public Task RemoveSubjectAsync(Guid Id);
    public Task<List<Subject>> GetAllSubjectsPageAsync(int page = 0, int take = 10);
    public Task<List<Subject>> GetAllSubjectsAsync();


}