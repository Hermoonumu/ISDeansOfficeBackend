using DeanInfoSystem.Domain;
namespace DeanInfoSystem.Application.DTO;

public class UserUpdateDTO
{
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Username { set; get; }
    public DateTime BirthDate { set; get; }
    public Position Position { set; get; }

    public Guid? ProgramId { set; get; }
    public EdProgram? Program { set; get; }


}