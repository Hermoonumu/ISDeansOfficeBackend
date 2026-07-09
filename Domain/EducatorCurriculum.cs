using System.ComponentModel.DataAnnotations;

namespace DeanInfoSystem.Domain;


public class EducatorCurriculum
{
    [Key]
    public Guid Id { set; get; }
    [Required]
    public Guid UserId { set; get; }
    public User? User { set; get; }
    [Required]
    public Guid CurriculumId { set; get; }
    public Curriculum? Curriculum { set; get; }
}