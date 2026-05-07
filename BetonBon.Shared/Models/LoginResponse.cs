using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models
{
    public record LoginResponse(string Token, string Username, UserRole Role, string refreshToken);
}
