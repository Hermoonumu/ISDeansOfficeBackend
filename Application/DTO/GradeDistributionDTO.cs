using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class GradeDistributionDTO
{
    public Guid? ProgramId { set; get; }
    public EdProgram? Program { set; get; }
    public int TotalGradeCount { set; get; }
    public List<GradeBucketsDTO>? Distribution { set; get; }

}
