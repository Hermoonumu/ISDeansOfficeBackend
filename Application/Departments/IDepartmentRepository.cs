using DeanInfoSystem.Domain;

public interface IDepartmentRepository
{
    public Task AddDepartmentAsync(Department dept);
    public Task<List<Department>> GetDepartmentsAsync();
    public Task<Department?> GetDepartmentByGuidAsync(string guid);
    public Task RemoveDepartmentAsync(Department department);
    public Task PersistChangesAsync();
}