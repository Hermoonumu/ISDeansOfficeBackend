using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

public interface IDepartmentService
{
    public Task<Department> NewDepartmentAsync(string name);
    public Task<List<Department>> GetAllDepartmentsAsync();
    public Task<Department?> GetDepartmentByGuidAsync(Guid guid);
    public Task PatchDepartmentAsync(Guid guid, JsonPatchDocument<Department> Patch);
    public Task RemoveDepartmentAsync(Guid guid);
}