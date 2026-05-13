using BetonBon.Domain.Users;

namespace BetonBon.Application
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}