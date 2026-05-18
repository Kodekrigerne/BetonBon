using BetonBon.Shared.Enums;

namespace BetonBon.Domain.Users
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; private set; }
        public PasswordHash HashedPassword { get; private set; }
        public UserRole Role { get; private set; }
        public int EmployeeNumber { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        // Parameterless constructor for EF purposes
#pragma warning disable CS8618
        private User() { }
#pragma warning restore CS8618

        private User(string username, PasswordHash hashedPassword, UserRole role, int employeeNumber)
        {
            ValidateUsername(username);
            if (employeeNumber <= 0) throw new ArgumentException("Employee number must be higher than zero.", nameof(employeeNumber));

            Id = Guid.NewGuid();
            Username = username;
            HashedPassword = hashedPassword;
            Role = role;
            EmployeeNumber = employeeNumber;
        }

        internal static User CreateUser(string username, PasswordHash hashedPassword, UserRole role, int employeeNumber)
        {
            return new User(username, hashedPassword, role, employeeNumber);
        }

        public void Update(string username, UserRole role)
        {
            ValidateUsername(username);
            Username = username;
            Role = role;
        }

        public void SetPassword(string password, IPasswordHasher passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password must be atleast 8 characters long.", nameof(password));
            }

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

        public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
    }
}