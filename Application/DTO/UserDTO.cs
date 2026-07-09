using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.DTO;



public class UserDTO
{
    public required Guid Id { set; get; }
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Username { set; get; }
    public DateTime BirthDate { set; get; }
    public Position Position { set; get; }

}