using BetonBon.Application;
using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure.Services;
using BetonBon.Infrastructure.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BetonBon.Infrastructure
{
    public static class DependencyInjection
    {
        extension(IServiceCollection collection)
        {
            public IServiceCollection AddInfrastructureServices()
            {
                collection.AddScoped<IUserRepository, UserRepository>();
                collection.AddScoped<IPasswordHasher, PasswordHasher>();
                collection.AddScoped<IQueryDispatcher, QueryDispatcher>();
                collection.AddScoped<ICommandDispatcher, CommandDispatcher>();

                return collection;
            }
        }
    }
}
