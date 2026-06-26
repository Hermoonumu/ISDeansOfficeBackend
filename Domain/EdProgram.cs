using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanInfoSystem.Domain;


public class EdProgram
{
    [Key]
    public Guid Id {set;get;}
    [Required]
    [StringLength(255)]
    public required string ProgramName{set;get;}
    public Guid? DepartmentId{set;get;}
    public Department? Department {set;get;}
}