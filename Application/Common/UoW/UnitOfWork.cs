using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeanInfoSystem.Application.Common.UoW;



public class UnitOfWork(SystemDbContext _db) : IUnitOfWork
{
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        IDbContextTransaction tr = await _db.Database.BeginTransactionAsync();
        return tr;
    }

    public async Task CommitTransactionAsync()
    {
        await _db.Database.CommitTransactionAsync();
    }

    public bool IsTransaction()
    {
        return _db.Database.CurrentTransaction == null;
    }

    public async Task RollbackTransactionAsync()
    {
        await _db.Database.RollbackTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}