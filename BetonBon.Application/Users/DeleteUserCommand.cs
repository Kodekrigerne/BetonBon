namespace BetonBon.Application.Users
{
    public record DeleteUserCommand(Guid Id) : ICommand;
}
