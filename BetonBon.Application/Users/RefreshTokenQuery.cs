using BetonBon.Shared.Models;

namespace BetonBon.Application.Users
{
    public record RefreshTokenQuery(string Token, string RefreshToken) : IQuery<LoginResponse>;
}
