using System.Net.Http.Headers;

namespace BetonBon.API.Tests
{
    public class TestAuthHandler : DelegatingHandler
    {
        public string? Token { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            return await base.SendAsync(request, cancellation);
        }
    }
}
