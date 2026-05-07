using BetonBon.Application.Users;
using BetonBon.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BetonBon.Application
{
    public static class DependencyInjection
    {
        extension(IServiceCollection collection)
        {
            public IServiceCollection AddApplicationServices()
            {
                collection.AddScoped<UserFactory>();
                collection.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();

                return collection;
            }
        }
    }
}
