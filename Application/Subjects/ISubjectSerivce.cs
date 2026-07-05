using DeanInfoSystem.Application.DTO;

namespace DeanInfoSystem.Application.Subjects;


public interface ISubjectService
{
    public Task<Guid> AddSubjectAsync(NewSubjectDTO nsDTO);
    public Task ChangeSubjectNameAsync(string Id, string name);
    public Task RemoveSubjectAsync(string Id);

}