using BetonBon.Shared.Enums;

namespace BetonBon.Application.Users
{
    public record CreateUserCommand(string Username, string Password, UserRole Role) : ICommand<Guid>;
}
