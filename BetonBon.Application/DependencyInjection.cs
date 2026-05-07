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

                return collection;
            }
        }
    }
}
