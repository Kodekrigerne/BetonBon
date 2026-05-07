using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models
{
    public record CreateUserDTO(string Username, string Password, UserRole Role);
}
