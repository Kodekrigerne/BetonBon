using System.Security.Authentication;
using System.Security.Claims;
using BetonBon.Application;
using BetonBon.Application.Authentication;
using BetonBon.Shared.Models.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure.Authentication
{
    public class RefreshTokenQueryHandler : IQueryHandler<RefreshTokenQuery, LoginResponse>
    {
        private readonly BetonBonDbContext _db;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenQueryHandler(BetonBonDbContext db, IJwtTokenService jwtTokenService)
        {
            _db = db;
            _jwtTokenService = jwtTokenService;
        }

        async Task<LoginResponse?> IQueryHandler<RefreshTokenQuery, LoginResponse>.HandleAsync(RefreshTokenQuery query)
        {
            var principal = await _jwtTokenService.GetPrincipalFromExpiredToken(query.Token);
            var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new AuthenticationException("Invalid token");

            var userId = Guid.Parse(userIdString);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null ||
                user.RefreshToken != query.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new AuthenticationException("Invalid refresh token");
            }

            var newToken = _jwtTokenService.GenerateJwtToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.UpdateRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
            await _db.SaveChangesAsync();

            return new LoginResponse(newToken, user.Username, user.Role, newRefreshToken);
        }
    }
}
