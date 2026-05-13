using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonAPI
    {
        [Post("/createUser")]
        Task<Guid> CreateUser(CreateUserDTO userToCreate);

        [Get("/viewUsers")]
        Task<List<UserDto>> GetAllUsers();

        [Post("/login")]
        Task<LoginResponse?> LoginUser(UserLoginDto userToLogin);
    }
}