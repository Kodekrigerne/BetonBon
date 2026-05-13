using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonApi
    {
        [Post("/createUser")]
        Task<Guid> CreateUser(CreateUserDTO userToCreate);

        [Get("/viewUsers")]
        Task<List<UserDto>> GetAllUsers();

        [Put("/updateUser")]
        Task UpdateUser(UpdateUserDTO user);

        [Delete("/deleteUser/{id}")]
        Task DeleteUser(Guid id);

        [Post("/login")]
        Task<LoginResponse?> LoginUser(UserLoginDto userToLogin);
    }
}
