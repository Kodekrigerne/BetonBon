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

            builder.Services
                .AddRefitClient<IBetonBonAPI>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("http://localhost:5281");
                });


            await builder.Build().RunAsync();
        }
    }
}
