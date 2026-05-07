using BetonBon.Application;
using BetonBon.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BetonBon.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JsonWebTokenHandler _handler;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings, JsonWebTokenHandler handler)
        {
            _jwtSettings = jwtSettings.Value;
            _handler = handler;
        }

        string IJwtTokenService.GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _handler.CreateToken(descriptor);

            return token;
        }
    }
}
