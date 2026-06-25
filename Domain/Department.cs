using System.ComponentModel.DataAnnotations;

namespace DeanInfoSystem.Domain;


public class Department
{
    [Required]
    [Key]
    public Guid Id{set;get;}
    [Required]
    [StringLength(255)]
    public required string DepartmentName{set;get;}
}