using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BetonBon.Application.Authentication;
using BetonBon.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

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
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim("employee_number", user.EmployeeNumber.ToString())
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

        string IJwtTokenService.GenerateRefreshToken()
        {
            byte[] refreshBytes = new byte[32];
            RandomNumberGenerator.Fill(refreshBytes);

            return Convert.ToBase64String(refreshBytes);
        }

        async Task<ClaimsPrincipal> IJwtTokenService.GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false
            };

            var handler = new JsonWebTokenHandler();
            var result = await handler.ValidateTokenAsync(token, tokenValidationParameters);

            if (!result.IsValid)
                throw new AuthenticationException("Invalid token");

            return new ClaimsPrincipal(result.ClaimsIdentity);
        }
    }
}
