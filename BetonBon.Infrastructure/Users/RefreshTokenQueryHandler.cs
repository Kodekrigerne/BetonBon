using System.Security.Authentication;
using BetonBon.Application;
using BetonBon.Application.Users;
using BetonBon.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure.Users
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
            var username = principal.Identity?.Name
                ?? principal.FindFirst("unique_name")?.Value
                ?? throw new AuthenticationException("Invalid token");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

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
