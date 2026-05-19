using BetonBon.Shared.Models.UserModels;

namespace BetonBon.Application.Users
{
    public record LoginQuery(string Username, string Password) : IQuery<LoginResponse>;
}
