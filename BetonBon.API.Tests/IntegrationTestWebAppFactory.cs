using BetonBon.Application;
using BetonBon.Client.RefitInterfaces;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Testcontainers.PostgreSql;

namespace BetonBon.API.Tests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:18-alpine")
            .WithDatabase("betonbon_test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public TestAuthHandler AuthHandler { get; } = new TestAuthHandler();

        public async ValueTask InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using (var scope = this.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

                await dbContext.Database.EnsureCreatedAsync();
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BetonBonDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<BetonBonDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
        }

        public IBetonBonApi CreateRefitClient()
        {
            //var serverHandler = this.Server.CreateHandler();
            //var clientWithAuth = HttpClientFactory.Create(serverHandler, AuthHandler);

            //clientWithAuth.BaseAddress = this.ClientOptions.BaseAddress;

            var clientWithAuth = CreateDefaultClient(AuthHandler);

            return RestService.For<IBetonBonApi>(clientWithAuth);
        }

        public async Task<string> GetValidAdminTokenAsync()
        {
            using (var scope = this.Services.CreateScope())
            {
                var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
                var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

                var adminUser = await userFactory.CreateAsync("admin", "adminpassword", UserRole.Admin, 1000);
                var token = jwtTokenService.GenerateJwtToken(adminUser);

                return token;
            }
        }

        public async Task<string> GetValidUserTokenAsync()
        {
            using (var scope = this.Services.CreateScope())
            {
                var userFactory = scope.ServiceProvider.GetRequiredService<UserFactory>();
                var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

                var userUser = await userFactory.CreateAsync("user", "userpassword", UserRole.User, 2000);
                var token = jwtTokenService.GenerateJwtToken(userUser);

                return token;
            }
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
