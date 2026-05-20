using BetonBon.Domain.Users;
using BetonBon.Shared;
using BetonBon.Shared.Enums;
using Moq;

namespace BetonBon.Domain.Tests.Users
{
    public class UserTests
    {

        [Fact]
        public void CreateUser_Given_ValidData_Then_CreatesUser()
        {
            // Arrange
            var username = "TestUser";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            // Act
            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);

            // Assert 
            Assert.Multiple(() =>
            {
                Assert.Equal(username, user.Username);
                Assert.Equal(role, user.Role);
                Assert.Equal(hashedPassword, user.HashedPassword);
                Assert.NotEqual(Guid.Empty, user.Id);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateUser_Given_EmptyUsername_Then_ThrowDomainException(string invalidUsername)
        {
            // Arrange
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                User.CreateUser(invalidUsername, hashedPassword, role, employeeNumber));
        }


        [Fact]
        public void CreateUser_Given_TooLongUsername_Then_ThrowDomainException()
        {
            // Arrange
            var invalidUsername = "dettebrugernavnerlængereend50characterslangtogbørkasteexception";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                User.CreateUser(invalidUsername, hashedPassword, role, employeeNumber));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void CreateUser_Given_EmployeeNumberIsZeroOrLess_Then_ThrowDomainException(int invalidNumber)
        {
            // Arrange
            var username = "Username";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                User.CreateUser(username, hashedPassword, role, invalidNumber));
        }

        [Fact]
        public void SetPassword_Given_ValidPassword_Then_UpdatesHashedPassword()
        {
            // Arrange
            var username = "Username";
            var role = UserRole.User;
            var initialHash = new PasswordHash("oldPassword", "oldSalt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, initialHash, role, employeeNumber);

            var validPassword = "SuperSecretPassword123";
            var expectedNewHash = new PasswordHash("newPassword", "newSalt");

            var hasherMock = new Mock<IPasswordHasher>();
            hasherMock
                .Setup(h => h.HashPassword(validPassword))
                .Returns(expectedNewHash);

            // Act
            user.SetPassword(validPassword, hasherMock.Object);

            // Assert
            Assert.Equal(expectedNewHash, user.HashedPassword);
        }

        [Fact]
        public void SetPassword_Given_PasswordIsTooShort_Then_ThrowDomainException()
        {
            // Arrange
            var username = "Username";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);
            var invalidPassword = "aa";
            var hasherMock = new Mock<IPasswordHasher>();

            // Act & Assert
            Assert.Throws<DomainException>(() => user.SetPassword(invalidPassword, hasherMock.Object));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void SetPassword_Given_EmptyPassword_Then_ThrowDomainException(string invalidPassword)
        {
            // Arrange
            var username = "Username";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);
            var hasherMock = new Mock<IPasswordHasher>();

            // Act & Assert
            Assert.Throws<DomainException>(() => user.SetPassword(invalidPassword, hasherMock.Object));
        }

        [Fact]
        public void Update_Given_ValidData_Then_UpdatesUserProperties()
        {
            // Arrange
            var username = "OldUsername";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);

            var newUsername = "NewUsername";
            var newRole = UserRole.Admin;

            // Act
            user.Update(newUsername, newRole);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(newUsername, user.Username);
                Assert.Equal(newRole, user.Role);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_Given_EmptyUsername_Then_ThrowDomainException(string invalidUsername)
        {
            // Arrange
            var username = "ValidUsername";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);
            var newRole = UserRole.Admin;

            // Act & Assert
            Assert.Throws<DomainException>(() => user.Update(invalidUsername, newRole));
        }

        [Fact]
        public void Update_Given_TooLongUsername_Then_ThrowDomainException()
        {
            // Arrange
            var username = "ValidUsername";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);
            var invalidUsername = "dettebrugernavnerlængereend50characterslangtogbørkasteexception";
            var newRole = UserRole.Admin;

            // Act & Assert
            Assert.Throws<DomainException>(() => user.Update(invalidUsername, newRole));
        }

        [Fact]
        public void UpdateRefreshToken_Given_ValidData_Then_UpdatesTokenProperties()
        {
            // Arrange
            var username = "Username";
            var role = UserRole.User;
            var hashedPassword = new PasswordHash("password", "salt");
            var employeeNumber = 1;

            var user = User.CreateUser(username, hashedPassword, role, employeeNumber);

            var newRefreshToken = "RefreshToken";
            var newExpiryTime = DateTime.UtcNow.AddDays(7);

            // Act
            user.UpdateRefreshToken(newRefreshToken, newExpiryTime);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(newRefreshToken, user.RefreshToken);
                Assert.Equal(newExpiryTime, user.RefreshTokenExpiryTime);
            });
        }


    }
}
