using BetonBon.API.RefitInterfaces;
using BetonBon.Application;
using BetonBon.Application.Users;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Infrastructure;
using BetonBon.Infrastructure.Services;
using BetonBon.Infrastructure.Users;
using BetonBon.Shared.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Refit;
using System.Security.Authentication;
using System.Text.Json.Serialization;

namespace BetonBon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.TraversePath().Load();

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(JwtSettings.SectionName));

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

            var apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
            var apiGrant = Environment.GetEnvironmentVariable("API_GRANT");

            var connectionString =
                $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Trust Server Certificate=true;";


            builder.Services
                    .AddRefitClient<IEconomicRelayApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri("https://apis.e-conomic.com/projectsapi/v1.1.0");
                        if (!string.IsNullOrEmpty(apiSecret))
                            c.DefaultRequestHeaders.Add("X-AppSecretToken", apiSecret);
                        if (!string.IsNullOrEmpty(apiGrant))
                            c.DefaultRequestHeaders.Add("X-AgreementGrantToken", apiGrant);
                    }
                    );

            builder.Services.AddDbContext<BetonBonDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            builder.Services
                .AddApplicationServices()
                .AddInfrastructureServices();


            builder.Services.AddScoped<IQueryHandler<LoginQuery, LoginResponse>, LoginQueryHandler>();
            builder.Services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddSingleton<JsonWebTokenHandler>();

            builder.Services.ConfigureHttpJsonOptions(options =>
                {
                    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Add services to the container.
            builder.Services.AddAuthorization();

            var clientUrl = builder.Configuration["ClientUrl"];

            builder.Services.AddCors(options => options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.WithOrigins(clientUrl!);
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                }));

            builder.Services.AddOpenApi();

            var app = builder.Build();

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
            app.UseCors("CustomPolicy");

            app.UseAuthorization();


            app.MapGet("/viewUsers", async (IQueryDispatcher dispatcher) =>
            {
                var users = await dispatcher.DispatchAsync<GetAllUsersQuery, List<UserDto>>(new GetAllUsersQuery());

                return Results.Ok(users);
            });

            // Get all projects
            app.MapGet("/api/projects", async (IEconomicRelayApi economicApi) =>
            {
                var response = await economicApi.GetProjectsAsync();
                return Results.Ok(response.Projects);
            }
            );

            app.MapPost("/createUser", async (ICommandDispatcher commandDispatcher, CreateUserDTO userToCreate) =>
            {
                var command = new CreateUserCommand(userToCreate.Username, userToCreate.Password, userToCreate.Role);

                var id = await commandDispatcher.DispatchAsync<CreateUserCommand, Guid>(command);

                return Results.Ok(id);
            });

            app.MapPost("/login", async (IQueryDispatcher queryDispatcher, UserLoginDto userLogin) =>
            {
                try
                {
                    var query = new LoginQuery(userLogin.Username, userLogin.Password);

                    var response = await queryDispatcher.DispatchAsync<LoginQuery, LoginResponse>(query);

                    return Results.Ok(response);
                }

                catch (AuthenticationException)
                {
                    return Results.Unauthorized();
                }
            });

            app.Run();
        }
    }
}