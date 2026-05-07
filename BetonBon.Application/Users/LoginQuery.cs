using BetonBon.Shared.Models;

namespace BetonBon.Application.Users
{
    public record LoginQuery(string Username, string Password) : IQuery<LoginResponse>;
}
