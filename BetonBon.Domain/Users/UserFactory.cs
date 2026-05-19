using BetonBon.Shared.Enums;

namespace BetonBon.Domain.Users
{
    public class UserFactory
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmployeeNumberUniqueValidator _employeeNumberUniqueValidator;

        public UserFactory(IPasswordHasher passwordHasher, IEmployeeNumberUniqueValidator employeeNumberUniqueValidator)
        {
            _passwordHasher = passwordHasher;
            _employeeNumberUniqueValidator = employeeNumberUniqueValidator;
        }

        public async Task<User> CreateAsync(string username, string password, UserRole role, int employeeNumber)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password must be atleast 8 characters long.", nameof(password));
            }

            if (await _employeeNumberUniqueValidator.ValidateUniqueEmployeeNumberAsync(employeeNumber))
                throw new ArgumentException("Employee Number must be unique", nameof(employeeNumber));

            var hashedPassword = _passwordHasher.HashPassword(password);

            return User.CreateUser(username, hashedPassword, role, employeeNumber);
        }
    }
}
