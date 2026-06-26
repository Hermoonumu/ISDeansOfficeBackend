using System.ComponentModel.DataAnnotations;

namespace DeanInfoSystem.Domain;


public class Department
{
    [Key]
    public Guid Id { set; get; }
    [Required]
    [MaxLength(255)]
    public required string DepartmentName { set; get; }
}