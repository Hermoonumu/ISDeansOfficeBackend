using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanInfoSystem.Domain;


public class Program
{
    [Required]
    [Key]
    public Guid Id {set;get;}
    [Required]
    [StringLength(255)]
    public required string ProgramName{set;get;}
    [ForeignKey("FK_Program_DepartmentId")]
    public Guid DepartmentId{set;get;}
    public Department? Department {set;get;}
}