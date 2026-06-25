using System.ComponentModel.DataAnnotations;

namespace DeanInfoSystem.Domain;



public class Subject
{
    [Required]
    [Key]
    public Guid Id{set;get;}
    [Required]
    [StringLength(255)]
    public required string SubjectName{set;get;}
}