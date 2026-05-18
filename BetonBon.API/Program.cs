using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;
using BetonBon.API.RefitInterfaces;
using BetonBon.Application;
using BetonBon.Application.Users;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Refit;

namespace BetonBon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.TraversePath().Load();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(JwtSettings.SectionName));

            var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

            var apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
            var apiGrant = Environment.GetEnvironmentVariable("API_GRANT");

            var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USER");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            var connectionString =
                $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Trust Server Certificate=true;";


            builder.Services
                    .AddRefitClient<IEconomicProjectsRelayApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri("https://apis.e-conomic.com/projectsapi/v1.1.0/");
                        if (!string.IsNullOrEmpty(apiSecret))
                            c.DefaultRequestHeaders.Add("X-AppSecretToken", apiSecret);
                        if (!string.IsNullOrEmpty(apiGrant))
                            c.DefaultRequestHeaders.Add("X-AgreementGrantToken", apiGrant);
                    }
                    );

            builder.Services
                    .AddRefitClient<IEconomicJournalsRelayApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri("https://apis.e-conomic.com/journalsapi/v14.0.1/");
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

            builder.Services.AddScoped<JsonWebTokenHandler>();

            builder.Services.ConfigureHttpJsonOptions(options =>
                {
                    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            var clientUrl = builder.Configuration["ClientUrl"];

            builder.Services.AddCors(options => options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.WithOrigins(clientUrl!);
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                }));

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Auto - migrates new migrations on startup, creates admin user if not present
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

                db.Database.Migrate();

                if (!db.Users.Any(u => u.Username == adminUsername))
                {
                    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                    var userFactory = new UserFactory(hasher, scope.ServiceProvider.GetRequiredService<IEmployeeNumberUniqueValidator>());

                    var adminUser = userFactory.Create(adminUsername!, adminPassword!, UserRole.Admin, 1);

                    db.Users.Add(adminUser);
                    db.SaveChanges();
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("CustomPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            // Get all projects
            app.MapGet("/api/projects", async (IEconomicProjectsRelayApi economicApi) =>
            {

                var response = await economicApi.GetProjectsAsync();
                return Results.Ok(response.Projects);
            })
            .RequireAuthorization();

            app.MapGet("/viewUsers", async (IQueryDispatcher dispatcher) =>
            {
                var users = await dispatcher.DispatchAsync<GetAllUsersQuery, List<UserDto>>(new GetAllUsersQuery());

                return Results.Ok(users);
            })
            .RequireAuthorization();

            app.MapPost("/createUser", async (ICommandDispatcher commandDispatcher, CreateUserDTO userToCreate) =>
            {
                var command = new CreateUserCommand(userToCreate.Username, userToCreate.Password, userToCreate.Role, userToCreate.EmployeeNumber);

                var id = await commandDispatcher.DispatchAsync<CreateUserCommand, Guid>(command);

                return Results.Ok(id);
            })
            .RequireAuthorization(nameof(UserRole.Admin));

            app.MapDelete("/deleteUser/{id}", async (ICommandDispatcher commandDispatcher, Guid id) =>
            {
                var command = new DeleteUserCommand(id);

                await commandDispatcher.DispatchAsync(command);

                return Results.NoContent();
            });

            app.MapPut("/updateUser", async (ICommandDispatcher commandDispatcher, UpdateUserDTO dto) =>
            {
                var command = new UpdateUserCommand(dto.Id, dto.Username, dto.Password, dto.Role);

                await commandDispatcher.DispatchAsync(command);

                return Results.NoContent();
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

            app.MapGet("/api/activitiesByProjectNumber", async (IEconomicProjectsRelayApi economicApi, int projectNumber) =>
            {
                var initialResponse = await economicApi.GetProjectActivitiesAsync(projectNumber);

                var projectActivities = initialResponse.ProjectActivities;

                List<ActivityDTO> activities = [];

                foreach (var activity in projectActivities)
                {
                    activities.Add(economicApi.GetActivityByNumberAsync(activity.ActivityNumber).Result);
                }

                return Results.Ok(activities);
            })
            .RequireAuthorization();

            app.MapGet("/api/materials", async (IEconomicProjectsRelayApi economicApi) =>
            {
                var response = await economicApi.GetAllMaterialsAsync();

                return Results.Ok(response.Materials);
            });

            app.MapPost("/api/newDraftEntry", async (IEconomicJournalsRelayApi economicApi, NewDraftEntryDTO entry) =>
            {
                var creationResponse = await economicApi.PostNewEntryAsync(entry);

                BookEntryNumberDTO entryNumber = new([creationResponse.CreatedEntryNumber]);

                var response = await economicApi.BookDraftEntryAsync(entryNumber);
                return Results.Ok(response.StatusCode);

            });

            app.MapPost("/api/newTimeEntry", async (TimeEntry timeEntry, IEconomicProjectsRelayApi economicApi, CancellationToken ct) =>
            {
                try
                {
                    var result = await economicApi.CreateTimeEntryAsync(timeEntry, ct);
                    return Results.Ok(result); //TODO: Results.Created(GET, result);
                }
                catch (ApiException ex)
                {
                    return Results.Problem(
                    detail: ex.Content,
                    statusCode: (int)ex.StatusCode
                    );
                }
            }).RequireAuthorization();

            app.MapGet("/api/employees", async (IEconomicProjectsRelayApi economicApi) =>
            {
                try
                {
                    var response = await economicApi.GetEmployeesAsync();
                    return Results.Ok(response);
                }
                catch (ApiException ex)
                {
                    return Results.Problem(
                    detail: ex.Content,
                    statusCode: (int)ex.StatusCode
                    );
                }
            }).RequireAuthorization(nameof(UserRole.Admin));

            app.Run();
        }
    }
}
