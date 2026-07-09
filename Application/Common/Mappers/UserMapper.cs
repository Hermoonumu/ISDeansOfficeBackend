using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Common.Mappers;



public static class UserMapper
{
    public static User DTOToUser(NewUserDTO nuDTO)
    {
        return new User()
        {
            FirstName = nuDTO.FirstName,
            LastName = nuDTO.LastName,
            Username = nuDTO.Username,
            BirthDate = nuDTO.BirthDate,
            PasswordHash = "",
            Position = nuDTO.Position
        };
    }
    public static UserDTO UserToDTO(User user)
    {
        return new UserDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            BirthDate = user.BirthDate,
            Position = user.Position
        };
    }
}