namespace DeanInfoSystem.Application.DTO;


using DeanInfoSystem.Domain;

public class AddSubjectToProgramDTO
{
    public Guid SubjectId { get; set; }
    public Semester Semester { get; set; }
    public int LectureHours { get; set; }
    public int PracticeHours { get; set; }
    public int LabHours { get; set; }
    public int CourseWorkHours { get; set; }
    public AssesmentType AssessmentType { get; set; }
}