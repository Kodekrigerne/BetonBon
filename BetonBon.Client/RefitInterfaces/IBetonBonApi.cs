using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Authentication;
using BetonBon.Shared.Models.UserModels;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonApi
    {
        [Post("/createUser")]
        Task<Guid> CreateUser(CreateUserRequest userToCreate);

        [Get("/viewUsers")]
        Task<List<UserDto>> GetAllUsers();

        [Put("/updateUser")]
        Task UpdateUser(UpdateUserRequest user);

        [Delete("/deleteUser/{id}")]
        Task DeleteUser(Guid id);

        [Post("/login")]
        Task<LoginResponse?> LoginUser(UserLoginDto userToLogin);
    }
}
