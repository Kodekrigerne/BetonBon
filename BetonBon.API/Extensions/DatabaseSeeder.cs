using BetonBon.Domain.Users;
using BetonBon.Infrastructure;
using BetonBon.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.API.Extensions
{
    public static class DatabaseSeeder
    {
        extension(WebApplication app)
        {
            public async Task ApplyMigrationsAndSeedAdmin(string adminUsername, string adminPassword)
            {
                // Auto - migrates new migrations on startup, creates admin user if not present
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<BetonBonDbContext>();

                    db.Database.Migrate();

                    if (!db.Users.Any(u => u.Username == adminUsername))
                    {
                        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                        var employeeNumberValidator = scope.ServiceProvider.GetRequiredService<IEmployeeNumberUniqueValidator>();
                        var userFactory = new UserFactory(hasher, employeeNumberValidator);

                        var adminUser = await userFactory.CreateAsync(adminUsername!, adminPassword!, UserRole.Admin, 1);

                        db.Users.Add(adminUser);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
