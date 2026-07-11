using DeanInfoSystem.API;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Analytics;



public class AnalyticsService(IAnalyticsRepository _analRepo,
                                IProgramRepository _progRepo) : IAnalyticsService
{
    public async Task<GradeDistributionDTO> GetGradeDistInProgramAsync(Guid ProgramId)
    {
        EdProgram program = await _progRepo.GetProgramByGuidAsync(ProgramId) ??
        throw new ProgramDoesntExistException("No such program");
        List<GradeBucketsDTO> gradeBuckets = await _analRepo.GetProgramGradeBucketsAsync(ProgramId);
        GradeDistributionDTO gdDTO = new()
        {
            ProgramId = ProgramId,
            Program = program,
            TotalGradeCount = gradeBuckets.Sum(gb => gb.Count),
            Distribution = gradeBuckets
        };

        foreach (GradeBucketsDTO gb in gradeBuckets)
        {
            gb.Percentage = (double)gb.Count / gdDTO.TotalGradeCount;
        }

        return gdDTO;
    }
}