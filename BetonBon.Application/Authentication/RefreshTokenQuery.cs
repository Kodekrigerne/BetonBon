using BetonBon.Shared.Models.Authentication;

namespace BetonBon.Application.Authentication
{
    public record RefreshTokenQuery(string Token, string RefreshToken) : IQuery<LoginResponse>;
}
