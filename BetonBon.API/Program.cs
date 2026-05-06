using BetonBon.Application;
using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Infrastructure.Services;
using BetonBon.Infrastructure.Users;
using BetonBon.Shared.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            Env.TraversePath().Load();

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

            var connectionString =
                $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Trust Server Certificate=true;";

            builder.Services.AddDbContext<BetonBonDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<UserFactory>();
            builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();


            builder.Services.AddScoped<IQueryHandler<GetAllUsersQuery, List<UserDto>>, GetAllUsersQueryHandler>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();


            app.MapGet("allUsers", async (IQueryDispatcher dispatcher) =>
            {
                var users = await dispatcher.DispatchAsync<GetAllUsersQuery, List<UserDto>>(new GetAllUsersQuery());

                return Results.Ok(users);
            });


            // Auto - migrates new migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
