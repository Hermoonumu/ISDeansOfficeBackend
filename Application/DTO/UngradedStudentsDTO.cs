using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class UngradedStudentsDTO
{
    public string GradeId { set; get; }
    public string StudentId { set; get; }
    public string FullName { set; get; }
    public string CurriculumId { set; get; }
    public string SubjectName { set; get; }
    public Status Status { set; get; }
    public int? Grade { set; get; }
}