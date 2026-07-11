namespace DeanInfoSystem.Application.DTO;

using DeanInfoSystem.Domain;

public class StudentGradeDTO
{
    public Guid Id { set; get; }
    public Guid? StudentId { set; get; }
    public UserDTO? Student { set; get; }
    public Guid? CurriculumId { set; get; }
    public Curriculum? Curriculum { set; get; }
    public Status Status { set; get; }
    public int? Grade { set; get; }
    public DateTime? PassedDate { set; get; }
    public Guid? GradedById { set; get; }
    public UserDTO? GradedBy { set; get; }

}