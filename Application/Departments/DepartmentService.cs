using System.Transactions;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.UoW;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

public class DepartmentService(IDepartmentRepository _deptRepo,
                                IUnitOfWork _uow) : IDepartmentService
{
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _deptRepo.GetDepartmentsAsync();
    }

    public async Task<Department?> GetDepartmentByGuidAsync(Guid guid)
    {
        return await _deptRepo.GetDepartmentByGuidAsync(guid);
    }

    public async Task<Department> NewDepartmentAsync(string name)
    {
        Department dept = new Department() { DepartmentName = name };

        using var tr = await _uow.BeginTransactionAsync();
        await _deptRepo.AddDepartmentAsync(dept);
        await _uow.SaveChangesAsync();
        tr.Commit();
        return dept;

    }

    public async Task PatchDepartmentAsync(Guid guid, JsonPatchDocument<Department> Patch)
    {
        Department? user = await _deptRepo.GetDepartmentByGuidAsync(guid) ??
        throw new DepartmentDoesntExistException("No such department");

        using var tr = await _uow.BeginTransactionAsync();
        Patch.ApplyTo(user, (err) =>
        {
            tr.Rollback();
            throw new UpdateFailedException($"Department update failed.\nAmended object: {err.AffectedObject.GetType()}\nError msg: {err.ErrorMessage}");
        });
        await _uow.SaveChangesAsync();
        tr.Commit();
    }

    public async Task RemoveDepartmentAsync(Guid guid)
    {
        Department? dept = await _deptRepo.GetDepartmentByGuidAsync(guid) ??
                            throw new DepartmentDoesntExistException("No such department");
        await _deptRepo.RemoveDepartmentAsync(dept);
    }
}