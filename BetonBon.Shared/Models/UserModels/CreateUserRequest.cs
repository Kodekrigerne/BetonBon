using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models.UserModels
{
    public record CreateUserRequest(string Username, string Password, UserRole Role, int EmployeeNumber);
}
