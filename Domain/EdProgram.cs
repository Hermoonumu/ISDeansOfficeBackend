using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanInfoSystem.Domain;


public class EdProgram
{
    [Key]
    public Guid Id { set; get; }
    [Required]
    [MaxLength(255)]
    public required string ProgramName { set; get; }
    [Required]
    [MaxLength(4)]
    public required string ProgramCode { set; get; }
    public ProgramStatus ProgramStatus { set; get; } = ProgramStatus.Drafted;
    public Guid? DepartmentId { set; get; }
    public Department? Department { set; get; }
}

public enum ProgramStatus
{
    Drafted,
    Rejected,
    Approved,
    Retired
}