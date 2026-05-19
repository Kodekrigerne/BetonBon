using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BetonBon.Client.Services;
using BetonBon.Shared.Models.Authentication;

namespace BetonBon.Client.Auth
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly LocalStorage _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthHeaderHandler(LocalStorage localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.LoadAsync("bb_token");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshed = await TryRefreshTokenAsync();
                if (refreshed)
                {
                    // Clone the request with the new token
                    var newToken = await _localStorage.LoadAsync("bb_token");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            var token = await _localStorage.LoadAsync("bb_token");
            var refreshToken = await _localStorage.LoadAsync("bb_refresh_token");

            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            try
            {
                var client = _httpClientFactory.CreateClient("RefreshClient");

                var refreshRequest = new RefreshTokenRequest(token!, refreshToken);
                var response = await client.PostAsJsonAsync("/refresh", refreshRequest);

                if (!response.IsSuccessStatusCode)
                    return false;

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                await _localStorage.SaveAsync("bb_token", loginResponse!.Token);
                await _localStorage.SaveAsync("bb_refresh_token", loginResponse.RefreshToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
