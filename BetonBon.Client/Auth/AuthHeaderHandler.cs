using BetonBon.Client.Services;
using System.Net.Http.Headers;

namespace BetonBon.Client.Auth
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly LocalStorage _localStorage;

        public AuthHeaderHandler(LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.LoadAsync("bb_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
