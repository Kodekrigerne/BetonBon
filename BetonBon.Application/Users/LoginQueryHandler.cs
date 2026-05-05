using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Domain.Users;

namespace BetonBon.Application.Users
{
    public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;


        public LoginQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public Task<LoginResponse?> HandleAsync(LoginQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
