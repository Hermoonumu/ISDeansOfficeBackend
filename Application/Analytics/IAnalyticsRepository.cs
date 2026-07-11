using DeanInfoSystem.Application.DTO;

namespace DeanInfoSystem.Application.Analytics;


public interface IAnalyticsRepository
{
    public Task<List<GradeBucketsDTO>> GetProgramGradeBucketsAsync(Guid ProgramId);

}