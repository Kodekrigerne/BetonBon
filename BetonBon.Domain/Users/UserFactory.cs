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
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            }

            if (username.Length > 20)
            {
                throw new ArgumentException("Username cannot be longer than 20 characters.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password must be atleast 8 characters long.", nameof(password));
            }

            var hashedPassword = _passwordHasher.HashPassword(password);

            return User.CreateUser(username, hashedPassword, role);
        }
    }
}
