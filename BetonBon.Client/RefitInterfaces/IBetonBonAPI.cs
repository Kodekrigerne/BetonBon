using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonAPI
    {
        [Post("/createUser")]
        Task<Guid> CreateUser(CreateUserDTO userToCreate);
    }
}