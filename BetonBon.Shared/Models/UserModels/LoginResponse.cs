using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models.UserModels
{
    public record LoginResponse(string Token, string Username, UserRole Role, string RefreshToken);
}
