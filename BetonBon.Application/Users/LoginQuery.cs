using BetonBon.Shared.Enums;

namespace BetonBon.Application.Users
{
    public record LoginQuery(string Username, string Password) : IQuery<LoginResponse>;

    public record LoginResponse(string Token, string Username, UserRole Role);
}
