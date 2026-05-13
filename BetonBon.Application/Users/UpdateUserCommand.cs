using BetonBon.Shared.Enums;

namespace BetonBon.Application.Users
{
    public record UpdateUserCommand(Guid Id, string Username, string? Password, UserRole Role) : ICommand;
}
