using BetonBon.Shared.Enums;

<<<<<<<< HEAD:BetonBon.Shared/Models/Authentication/LoginResponse.cs
namespace BetonBon.Shared.Models.Authentication
========
namespace BetonBon.Shared.Models.UserModels
>>>>>>>> dev:BetonBon.Shared/Models/UserModels/LoginResponse.cs
{
    public record LoginResponse(string Token, string Username, UserRole Role, string RefreshToken);
}
