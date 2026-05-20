using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IBetonBonApi
    {
        [Post("/createUser")]
        Task<IApiResponse<Guid>> CreateUser(CreateUserDTO userToCreate);

        [Get("/viewUsers")]
        Task<List<UserDto>> GetAllUsers();

        [Put("/updateUser")]
        Task UpdateUser(UpdateUserDTO user);

        [Delete("/deleteUser/{id}")]
        Task DeleteUser(Guid id);

        [Post("/login")]
        Task<IApiResponse<LoginResponse>?> LoginUser(UserLoginDto userToLogin);
    }
}
