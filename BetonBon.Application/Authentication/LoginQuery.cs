using BetonBon.Shared.Models.Authentication;

namespace BetonBon.Application.Authentication
{
    public record LoginQuery(string Username, string Password) : IQuery<LoginResponse>;
}
