using BetonBon.Shared.Models.UserModels;

namespace BetonBon.Application.Users.UserQueries
{
    public record GetAllUsersQuery : IQuery<List<UserResponse>>;
}
