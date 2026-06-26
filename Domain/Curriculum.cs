using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeanInfoSystem.Domain;



public class Curriculum
{
    [Key]
    public Guid Id{set;get;}
    public Guid? ProgramId{set;get;}
    public EdProgram? EdProgram{set;get;}
    public Guid? SubjectId{set;get;}
    public Subject? Subject{set;get;}
    public Semester? Semester{set;get;}
    public int LectureHours{set;get;}
    public int PracticeHours{set;get;}
    public int LabHours{set;get;}
    public int CourseWorkHours{set;get;}
    public AssesmentType? AssesmentType{set;get;}


}


public enum Semester
{
    FirstYearFirstSemester,
    FirstYearSecondSemester,
    SecondYearFirstSemester,
    SecondYearSecondSemester,
    ThirdYearFirstSemester,
    ThirdYearSecondSemester,
    FourthYearFirstSemester,
    FourthYearSecondSemester
}


public enum AssesmentType
{
    Credit,
    Exam
}