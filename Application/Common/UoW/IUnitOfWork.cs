using System.Data;

namespace DeanInfoSystem.Application.Common.UoW;



public interface IUnitOfWork
{
    public Task<IDbTransaction> BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
    public Task SaveChangesAsync();

    public bool IsTransaction();

}