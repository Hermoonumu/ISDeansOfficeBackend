using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class ProgramPatchDTO
{
    public string ProgramName { set; get; }
    public string ProgramCode { set; get; }
    public ProgramStatus ProgramStatus { set; get; } = ProgramStatus.Drafted;
    public Guid? DepartmentId { set; get; }
}