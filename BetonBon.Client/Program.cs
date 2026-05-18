using BetonBon.Client.Auth;
using BetonBon.Client.Pages.StopwatchRegistration;
using BetonBon.Client.RefitInterfaces;
using BetonBon.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
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
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddRefitClient<IBetonBonApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = backendApiUrl;
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<LocalStorage>();
            builder.Services.AddTransient<AuthHeaderHandler>();
            builder.Services.AddScoped<PopupService>();
            builder.Services.AddScoped<TimeEntryService>();

            await builder.Build().RunAsync();
        }
    }
}
