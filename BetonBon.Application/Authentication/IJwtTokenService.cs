using System.Security.Claims;
using BetonBon.Domain.Users;

namespace BetonBon.Application.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    }
}
