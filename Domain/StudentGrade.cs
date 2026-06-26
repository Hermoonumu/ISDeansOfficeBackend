using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DeanInfoSystem.Domain;



public class StudentGrade
{
    [Key]
    public Guid Id{set;get;}
    [Required]
    public required Guid? StudentId{set;get;}
    public User? Student {set;get;}
    [Required]
    public Guid? CurriculumId {set;get;}
    public Curriculum? Curriculum{set;get;}
    [Required]
    public required Status Status {set;get;}
    public int? Grade {set;get;}
    [AllowNull]
    public DateTime? PassedDate {set;get;}

}

public enum Status
{
    Passed,
    Failed,
    Pending
}