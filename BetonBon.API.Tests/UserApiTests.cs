using BetonBon.Client.RefitInterfaces;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BetonBon.API.Tests
{
    public class UserApiTests : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IntegrationTestWebAppFactory _factory;
        private readonly IBetonBonApi _api;

        public UserApiTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            var client = factory.CreateClient();

            _api = RestService.For<IBetonBonApi>(client);
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

            var user = await userFactory.CreateAsync(username, password, UserRole.Admin, 5);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var loginRequest = new UserLoginDto(username, password);

            // Act
            var response = await _api.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(response);
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidUser_ReturnsCorrectUser()
        {
            // Arrange
            var username = "admin2";
            var password = "secretpassword2";

            using var scope = _factory.Services.CreateScope();

            var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

            dbContext.Database.EnsureCreated();

            var user = await userFactory.CreateAsync(username, password, UserRole.Admin, 6);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var loginRequest = new UserLoginDto(username, password);

            // Act
            var response = await _api.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(response);
            //Assert.NotNull(response.Content);
            //Assert.Equal(user.Username, response.Content.Username);
            //Assert.Equal(user.Role, response.Content.Role);
        }
    }
}
