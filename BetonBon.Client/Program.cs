using BetonBon.Client.RefitInterfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

namespace BetonBon.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var backendApiUrl = builder.Configuration["BackendApiUrl"];

            Uri apiBaseAddress;

            if (!string.IsNullOrEmpty(backendApiUrl))
            {
                apiBaseAddress = new Uri(backendApiUrl);
            }
            else
            {
                apiBaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/");
            }

            builder.Services.AddRefitClient<IEconomicApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://localhost:7155");
                });

            builder.Services.AddRefitClient<IBetonBonApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://localhost:7155");
                });

            await builder.Build().RunAsync();
        }
    }
}
