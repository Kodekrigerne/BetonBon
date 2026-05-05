using BetonBon.Shared.Enums;

namespace BetonBon.Domain.Users
{
    public class UserFactory
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserFactory(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public User Create(string username, string password, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password must be atleast 8 characters long.", nameof(password));
            }

            var hashedPassword = _passwordHasher.HashPassword(password);

            return User.CreateUser(username, hashedPassword, role);
        }
    }
}
