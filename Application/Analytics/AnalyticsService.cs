using DeanInfoSystem.API;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;

namespace DeanInfoSystem.Application.Analytics;



public class AnalyticsService(IAnalyticsRepository _analRepo,
                                IProgramRepository _progRepo) : IAnalyticsService
{
    public async Task<GradeDistributionDTO> GetGradeDistInProgramAsync(Guid ProgramId)
    {
        List<GradeBucketsDTO> gradeBuckets = await _analRepo.GetProgramGradeBucketsAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");
        GradeDistributionDTO gdDTO = new()
        {
            ProgramId = ProgramId,
            Program = await _progRepo.GetProgramByGuidAsync(ProgramId),
            TotalGradeCount = gradeBuckets.Sum(gb => gb.Count),
            Distribution = gradeBuckets
        };

        foreach (GradeBucketsDTO gb in gradeBuckets)
        {
            gb.Percentage = gb.Count / gdDTO.TotalGradeCount;
        }

        return gdDTO;
    }
}