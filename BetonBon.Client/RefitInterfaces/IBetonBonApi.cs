using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonApi
    {
        [Post("/createUser")]
        Task<Guid> CreateUser(CreateUserDTO userToCreate);
    }
}
