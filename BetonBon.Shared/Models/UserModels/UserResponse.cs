using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models.UserModels
{
    public record UserResponse(Guid Id, string Username, UserRole Role, int EmployeeNumber, byte[] RowVersion);

}
