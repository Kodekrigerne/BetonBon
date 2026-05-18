using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models
{
    public record UserDto(Guid Id, string Username, UserRole Role, int EmployeeNumber);

}
