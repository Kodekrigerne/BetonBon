using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models.UserModels
{
    public record UpdateUserDTO(Guid Id, string Username, string? Password, UserRole Role);
}
