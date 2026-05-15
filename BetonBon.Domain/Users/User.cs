using BetonBon.Shared.Enums;

namespace BetonBon.Domain.Users
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; private set; }
        public PasswordHash HashedPassword { get; private set; }
        public UserRole Role { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        // Parameterless constructor for EF purposes
        private User() { }

        private User(string username, PasswordHash hashedPassword, UserRole role)
        {
            ValidateUsername(username);

            Id = Guid.NewGuid();
            Username = username;
            HashedPassword = hashedPassword;
            Role = role;
        }

        internal static User CreateUser(string username, PasswordHash hashedPassword, UserRole role)
        {
            return new User(username, hashedPassword, role);
        }

        public void Update(string username, UserRole role)
        {
            ValidateUsername(username);
            Username = username;
            Role = role;
        }

        public void SetPassword(string password, IPasswordHasher passwordHasher)
        {
            ValidatePassword(password);
            HashedPassword = passwordHasher.HashPassword(password);
        }

        private static void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            }

            if (username.Length > 50)
            {
                throw new ArgumentException("Username cannot be longer than 20 characters.", nameof(username));
            }
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }
            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));
            }
        }

        public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
    }
}