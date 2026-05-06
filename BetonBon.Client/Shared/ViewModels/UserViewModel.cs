using BetonBon.Shared.Enums;

namespace BetonBon.Client.Shared.ViewModels
{
    public record UserViewModel(Guid Id, string Name, UserRole Role);
}
