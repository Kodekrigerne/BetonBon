using BetonBon.Client.RefitInterfaces;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace BetonBon.API.Tests.UserEndpointsTests
{
    public class CreateUserEndpointTests : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestWebAppFactory _factory;
        private readonly IBetonBonApi _api;

        public CreateUserEndpointTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            _api = factory.CreateRefitClient();
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
            _factory.AuthHandler.Token = await _factory.GetValidAdminTokenAsync();

            var newUserDto = new CreateUserDTO("newAdmin", "newPassword", UserRole.User, 10);

            using var scope = _factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            // Act
            var response = await _api.CreateUser(newUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_WithUserAuthorization_ReturnsStatus401()
        {
            // Arrange
            _factory.AuthHandler.Token = await _factory.GetValidUserTokenAsync();

            var newUserDto = new CreateUserDTO("newUser", "newPassword", UserRole.User, 10);

            using var scope = _factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            // Act
            var response = await _api.CreateUser(newUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
