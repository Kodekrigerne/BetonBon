using BetonBon.Shared.Enums;

namespace BetonBon.Client.Shared.UserModels.ViewModels
{
    public record UserViewModel(Guid Id, string Name, UserRole Role, int EmployeeNumber);
}
