using System.Security.Claims;
using System.Text.Json;
using BetonBon.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

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
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
