using System.Transactions;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Subjects;

public class SubjectService(ISubjectRepository _subjRepo,
                            IUnitOfWork _uow) : ISubjectService
{
    public async Task<Guid> AddSubjectAsync(NewSubjectDTO nsDTO)
    {
        Subject subject = new Subject()
        {
            SubjectName = nsDTO.SubjectName
        };
        using var tr = await _uow.BeginTransactionAsync();
        if (!await _subjRepo.IsSubjectNameTaken(nsDTO.SubjectName))
        {
            await _subjRepo.AddSubjectAsync(subject);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();
            return subject.Id;
        }
        await _uow.RollbackTransactionAsync();
        throw new SubjectAlreadyExistsException("This subject already exists");
    }

    public async Task ChangeSubjectNameAsync(Guid Id, string name)
    {
        Subject? subject = await _subjRepo.GetSubjectByGuidAsync(Id) ??
        throw new SubjectDoesntExistException("No such subject");
        using var tr = await _uow.BeginTransactionAsync();
        if (!await _subjRepo.IsSubjectNameTaken(name))
        {
            subject.SubjectName = name;
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();
            return;
        }
        await _uow.RollbackTransactionAsync();
        throw new SubjectAlreadyExistsException("This subject name is taken");
    }


    public async Task RemoveSubjectAsync(Guid Id)
    {
        await _subjRepo.RemoveSubjectAsync(Id);
    }

    public async Task<List<Subject>> GetAllSubjectsPageAsync(int page = 0, int take = 10)
    {
        return await _subjRepo.GetAllSubjectsPageAsync(page, take);
    }

    public async Task<List<Subject>> GetAllSubjectsAsync()
    {
        return await _subjRepo.GetAllSubjectsAsync();
    }
}