using BetonBon.Shared.Enums;

namespace BetonBon.Domain.Users
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; private set; }
        public PasswordHash HashedPassword { get; private set; }
        public UserRole Role { get; private set; }

        // Parameterless constructor for EF purposes
        public User() { }

        private User(string username, PasswordHash hashedPassword, UserRole role)
        {
            Id = Guid.NewGuid();
            Username = username;
            HashedPassword = hashedPassword;
            Role = role;
        }

        internal static User CreateUser(string username, PasswordHash hashedPassword, UserRole role)
        {
            return new User(username, hashedPassword, role);
        }
    }
}
