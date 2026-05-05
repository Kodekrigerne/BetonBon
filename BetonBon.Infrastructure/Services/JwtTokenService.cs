using BetonBon.Application;
using BetonBon.Domain.Users;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BetonBon.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JsonWebTokenHandler _handler;

        public JwtTokenService(JwtSettings jwtSettings, JsonWebTokenHandler handler)
        {
            _jwtSettings = jwtSettings;
            _handler = handler;
        }

        string IJwtTokenService.GenerateJwtToken(User user)
        {
            //var descriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(),
            //            new Claim(ClaimTypes.Name, user.Username),
            //            )
            //    }
            //}

            return string.Empty;
        }
    }
}
