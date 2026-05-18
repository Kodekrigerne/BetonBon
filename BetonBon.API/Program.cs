using BetonBon.API.Endpoints;
using BetonBon.API.Extensions;
using BetonBon.API.RefitInterfaces;
using BetonBon.Application;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;
using System.Text.Json.Serialization;

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


            var apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
            var apiGrant = Environment.GetEnvironmentVariable("API_GRANT");

            var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USER");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            var connectionString = builder.Configuration.GetConnectionString();

            builder.Services.AddDbContext<BetonBonDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            builder.Services
                    .AddRefitClient<IEconomicProjectsApi>()
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
                    .AddRefitClient<IEconomicJournalsApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri("https://apis.e-conomic.com/journalsapi/v14.0.1/");
                        if (!string.IsNullOrEmpty(apiSecret))
                            c.DefaultRequestHeaders.Add("X-AppSecretToken", apiSecret);
                        if (!string.IsNullOrEmpty(apiGrant))
                            c.DefaultRequestHeaders.Add("X-AgreementGrantToken", apiGrant);
                    }
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

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy(nameof(UserRole.Admin), policy => policy.RequireRole(nameof(UserRole.Admin)));

            var clientUrl = builder.Configuration["ClientUrl"];

            builder.Services.AddCors(options => options.AddPolicy("CustomPolicy", policy =>
            {
                policy.AllowAnyOrigins();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }));

            builder.Services.AddOpenApi();


            var app = builder.Build();

            app.ApplyMigrationsAndSeedAdmin(adminUsername!, adminPassword!);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("CustomPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapUserEndpoints();
            app.MapEconomicEndpoints();

            app.Run();
        }
    }
}
