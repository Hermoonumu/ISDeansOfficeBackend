using DeanInfoSystem.Domain;
using Microsoft.EntityFrameworkCore;

public class DepartmentRepository(SystemDbContext _db) : IDepartmentRepository
{
    public async Task AddDepartmentAsync(Department dept)
    {
        await _db.Departments.AddAsync(dept);
        await _db.SaveChangesAsync();
    }

    public async Task<Department?> GetDepartmentByGuidAsync(string guid)
    {
        return await _db.Departments.Where(d => d.Id.ToString() == guid).FirstOrDefaultAsync();
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _db.Departments.ToListAsync();
    }

    public async Task PersistChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task RemoveDepartmentAsync(Department department)
    {
        await _db.Departments.Where(d => department.Id.ToString() == d.Id.ToString()).ExecuteDeleteAsync();
    }
}