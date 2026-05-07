using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Domain.Users;
using BetonBon.Shared.Enums;

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

            // Act
            var user = User.CreateUser(username, hashedPassword, role);

            // Assert 
            Assert.Multiple(() =>
            {
                Assert.Equal(username, user.Username);
                Assert.Equal(role, user.Role);
                Assert.Equal(hashedPassword, user.HashedPassword);
                Assert.NotEqual(Guid.Empty, user.Id);
            }
                );

        }
    }
}
