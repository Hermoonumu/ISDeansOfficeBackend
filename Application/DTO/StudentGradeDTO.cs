namespace DeanInfoSystem.Application.DTO;

using DeanInfoSystem.Domain;

public class StudentGradeDTO
{
    public Guid? Id { set; get; }
    public Guid? StudentId { set; get; }
    public UserDTO? Student { set; get; }
    public Guid? CurriculumId { set; get; }
    public Curriculum? Curriculum { set; get; }
    public string? SubjectName { set; get; }
    public Guid? ProgramId { set; get; }
    public string? ProgramName { set; get; }
    public Status Status { set; get; }
    public int? Grade { set; get; }
    public DateTime? GradingDate { set; get; }
    public Guid? GradedById { set; get; }
    public UserDTO? GradedBy { set; get; }

}