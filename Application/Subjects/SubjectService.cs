using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Subjects;

public class SubjectService(ISubjectRepository _subjRepo) : ISubjectService
{
    public async Task<Guid> AddSubjectAsync(NewSubjectDTO nsDTO)
    {
        Subject subject = new Subject()
        {
            SubjectName = nsDTO.SubjectName
        };
        if (!await _subjRepo.IsSubjectNameTaken(nsDTO.SubjectName))
        {
            await _subjRepo.AddSubjectAsync(subject);
            return subject.Id;
        }
        throw new SubjectAlreadyExistsException("This subject already exists");
    }

    public async Task ChangeSubjectNameAsync(Guid Id, string name)
    {
        Subject? subject = await _subjRepo.GetSubjectByGuidAsync(Id) ??
        throw new SubjectDoesntExistException("No such subject");
        if (!await _subjRepo.IsSubjectNameTaken(name))
        {
            subject.SubjectName = name;
        }
        await _subjRepo.PersistChangesAsync();
    }


    public async Task RemoveSubjectAsync(Guid Id)
    {
        await _subjRepo.RemoveSubjectAsync(Id);
    }
}