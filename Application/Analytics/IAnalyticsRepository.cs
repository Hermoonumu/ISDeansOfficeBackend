using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Analytics;


public interface IAnalyticsRepository
{
    public Task<List<GradeBucketsDTO>> GetProgramGradeBucketsAsync(Guid ProgramId);
    public Task<PerformanceDTO> GetStudentsPerformanceAsync(Guid UserId);
    public Task<int> GetStudentCountAsync();
    public Task<int> GetEducatorCountAsync();
    public Task<int> GetProgramCountAsync();
    public Task<List<Curriculum>> MissingEducatorCurriculaAsync();
    public Task<int> GetPendingGradeCountAsync();
    public Task<List<StudentDeanDashDTO>> GetScholarshipQualifyingAsync();
    public Task<List<StudentDeanDashDTO>> GetEndangeredStudentsAsync();


}