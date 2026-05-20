using BetonBon.Shared;
using BetonBon.Shared.Enums;
using System.ComponentModel.DataAnnotations;

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

        [Timestamp]
        public uint RowVersion { get; private set; }

        // Parameterless constructor for EF purposes
#pragma warning disable CS8618
        private User() { }
#pragma warning restore CS8618

        private User(string username, PasswordHash hashedPassword, UserRole role, int employeeNumber)
        {
            ValidateUsername(username);
            if (employeeNumber <= 0) throw new DomainException("Employee number must be higher than zero.", nameof(employeeNumber));

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
            ValidatePassword(password);
            HashedPassword = passwordHasher.HashPassword(password);
        }

        private static void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException("Username cannot be empty.", nameof(username));
            }

            if (username.Length > 50)
            {
                throw new DomainException("Username cannot be longer than 20 characters.", nameof(username));
            }
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException("Password cannot be empty.", nameof(password));
            }
            if (password.Length < 8)
            {
                throw new DomainException("Password must be at least 8 characters long.", nameof(password));
            }
        }

        public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
    }
}