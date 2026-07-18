using DeanInfoSystem.Application.DTO;

namespace DeanInfoSystem.Application.Analytics;


public interface IAnalyticsService
{
    public Task<GradeDistributionDTO> GetGradeDistInProgramAsync(Guid ProgramId);
    public Task<RankingDTO> GetStudentRankingInProgramAsync(Guid ProgramId);
    public Task<PerformanceDTO> GetStudentsPerformanceAsync(Guid UserId);
    public Task<EducatorDashboardDTO> GetEducatorDashboardAsync(Guid educatorId);


    public Task<DeanDashDTO> GetDeanDashboardAsync(Guid DeanId);
}