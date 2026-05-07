using BetonBon.Application;
using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Domain.Users;
using BetonBon.Infrastructure.Services;
using BetonBon.Infrastructure.Users;
using BetonBon.Shared.Models;
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

                collection.AddScoped<IQueryHandler<GetAllUsersQuery, List<UserDto>>, GetAllUsersQueryHandler>();

                return collection;
            }
        }
    }
}
