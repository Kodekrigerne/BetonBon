using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models.UserModels
{
    public record UserDto(Guid Id, string Username, UserRole Role, int EmployeeNumber);

}
