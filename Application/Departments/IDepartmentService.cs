using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

public interface IDepartmentService
{
    public Task<Department> NewDepartmentAsync(string name);
    public Task<List<Department>> GetAllDepartmentsAsync();
    public Task<Department?> GetDepartmentByGuidAsync(string guid);
    public Task PatchDepartmentAsync(string guid, JsonPatchDocument<Department> Patch);
    public Task RemoveDepartmentAsync(string guid);
}