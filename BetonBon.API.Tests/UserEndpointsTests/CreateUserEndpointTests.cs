using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace BetonBon.API.Tests.UserEndpointsTests
{
    public class CreateUserEndpointTests : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestWebAppFactory _factory;

        public CreateUserEndpointTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
        }

        public ValueTask InitializeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            await dbContext.Users.ExecuteDeleteAsync();

            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task CreateUser_WithAdminAuthorization_ReturnsStatus200()
        {
            // Arrange
            var adminToken = await _factory.GetValidAdminTokenAsync();
            var api = _factory.CreateRefitClient(adminToken);

            var newUserDto = new CreateUserRequest("newAdmin", "newPassword", UserRole.User, 10);

            // Act
            var response = await api.CreateUser(newUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_WithUserAuthorization_ReturnsStatus401()
        {
            // Arrange
            var userToken = await _factory.GetValidUserTokenAsync();
            var api = _factory.CreateRefitClient(userToken);

            var newUserDto = new CreateUserRequest("newUser", "newPassword", UserRole.User, 10);

            // Act
            var response = await api.CreateUser(newUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
