using DeanInfoSystem.Application.DTO;

namespace DeanInfoSystem.Application.Analytics;


public interface IAnalyticsService
{
    public Task<GradeDistributionDTO> GetGradeDistInProgramAsync(Guid ProgramId);
}