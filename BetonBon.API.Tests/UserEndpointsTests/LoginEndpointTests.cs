using BetonBon.Client.RefitInterfaces;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net;

namespace BetonBon.API.Tests.UserEndpointsTests
{
    public class LoginEndpointTests : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestWebAppFactory _factory;
        private readonly IBetonBonApi _api;

        public LoginEndpointTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            //_api = _factory.CreateRefitClient();
            var client = factory.CreateClient();
            _api = RestService.For<IBetonBonApi>(client);
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
        public async Task Login_WithValidUser_ReturnsStatus200()
        {
            // Arrange
            var username = "admin";
            var password = "secretpassword";

            using var scope = _factory.Services.CreateScope();

            var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            var user = await userFactory.CreateAsync(username, password, UserRole.Admin, 1);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var loginRequest = new UserLoginDto(username, password);

            // Act
            var response = await _api.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidUser_ReturnsCorrectUser()
        {
            // Arrange
            var username = "admin";
            var password = "secretpassword";

            using var scope = _factory.Services.CreateScope();

            var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            var user = await userFactory.CreateAsync(username, password, UserRole.Admin, 1);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var loginRequest = new UserLoginDto(username, password);

            // Act
            var response = await _api.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(user.Username, response.Content.Username);
            Assert.Equal(user.Role, response.Content.Role);
        }

        [Fact]
        public async Task Login_WithInvalidUser_ReturnsStatus401()
        {
            // Arrange
            var usernameInDb = "admin";
            var passwordInDb = "secretpassword";
            var invalidUsername = "invalidUser";
            var invalidPassword = "invalidPass";

            using var scope = _factory.Services.CreateScope();

            var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            var user = await userFactory.CreateAsync(usernameInDb, passwordInDb, UserRole.Admin, 1);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var loginRequest = new UserLoginDto(invalidUsername, invalidPassword);

            // Act
            var response = await _api.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
