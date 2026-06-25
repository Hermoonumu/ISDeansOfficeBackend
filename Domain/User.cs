using System.ComponentModel.DataAnnotations;

namespace DeanInfoSystem.Domain;


public class User
{
    [Required]
    [Key]
    public Guid Id {set;get;}
    [Required]
    [StringLength(255)]
    public required string FirstName{set;get;}
    [Required]
    [StringLength(255)]
    public required string LastName{set;get;}
    [Required]
    public required string PasswordHash{set;get;}
    public DateTime BirthDate{set;get;}
    [Required]
    public Position Position {set;get;}


}


public enum Position
{
    Dean, 
    ViceDean,
    Secretary,
    Assistant,
    EducationalAdvisor
}