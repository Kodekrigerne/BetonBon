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

            var backendApiUrl = new Uri(builder.Configuration["BackendApiUrl"]!);

            builder.Services.AddRefitClient<IEconomicApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = backendApiUrl;
                });

            builder.Services.AddRefitClient<IBetonBonApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = backendApiUrl;
                });

            await builder.Build().RunAsync();
        }
    }
}
