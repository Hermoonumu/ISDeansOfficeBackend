using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

public class DepartmentService(IDepartmentRepository _deptRepo) : IDepartmentService
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
        await _deptRepo.AddDepartmentAsync(dept);
        return dept;
    }

    public async Task PatchDepartmentAsync(Guid guid, JsonPatchDocument<Department> Patch)
    {
        Department? user = await _deptRepo.GetDepartmentByGuidAsync(guid) ??
        throw new DepartmentDoesntExistException("No such department");
        Patch.ApplyTo(user, (err) =>
        {
            throw new UpdateFailedException($"Department update failed.\nAmended object: {err.AffectedObject.GetType()}\nError msg: {err.ErrorMessage}");
        });
        await _deptRepo.PersistChangesAsync();
    }

    public async Task RemoveDepartmentAsync(Guid guid)
    {
        Department? dept = await _deptRepo.GetDepartmentByGuidAsync(guid) ??
                            throw new DepartmentDoesntExistException("No such department");
        await _deptRepo.RemoveDepartmentAsync(dept);
    }
}