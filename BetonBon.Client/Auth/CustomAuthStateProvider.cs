using System.Security.Claims;
using BetonBon.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BetonBon.Client.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorage _localStorage;

        public CustomAuthStateProvider(LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task UpdateAuthenticationStatus()
        {
            var authState = await GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.LoadAsync("bb_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwtToken(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwtToken(string jwt)
        {
            var handler = new JsonWebTokenHandler();
            var token = handler.ReadJsonWebToken(jwt);

            return token.Claims;
        }
    }
}
