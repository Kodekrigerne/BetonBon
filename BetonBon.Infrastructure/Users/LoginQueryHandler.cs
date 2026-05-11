using BetonBon.Application;
using BetonBon.Application.Users;
using BetonBon.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace BetonBon.Infrastructure.Users
{
    public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginResponse>
    {
        private readonly BetonBonDbContext _betonBonDb;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;


        public LoginQueryHandler(IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, BetonBonDbContext betonBonDb)
        {
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _betonBonDb = betonBonDb;
        }

        public async Task<LoginResponse?> HandleAsync(LoginQuery query)
        {
            var user = await _betonBonDb.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == query.Username);

            if (user == null)
                throw new AuthenticationException("Invalid username or password");

            if (!_passwordHasher.VerifyPassword(query.Password, user.HashedPassword))
                throw new AuthenticationException("Invalid username or password");

            var token = _jwtTokenService.GenerateJwtToken(user);

            return new LoginResponse(token, user.Username, user.Role);
        }
    }
}
