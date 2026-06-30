using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class NewUserDTO
{
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Username { set; get; }
    public required string Password { set; get; }
    public DateTime BirthDate { set; get; }
    public Position Position { set; get; }

}