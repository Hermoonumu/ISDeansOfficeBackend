using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeanInfoSystem.Application.Common.UoW;



public interface IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
    public Task SaveChangesAsync();

    public bool IsTransaction();

}