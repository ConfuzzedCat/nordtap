using System.Net;
using Blazored.LocalStorage;
using frontend.Services;
using frontend.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace frontend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Add services to the container.
        /* TODO: this isnt used in WASM, check for errors
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(options => { options.DetailedErrors = true; });
        */
        builder.Services.AddBlazoredLocalStorageAsSingleton();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

        builder.Services.AddScoped(sp => new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            })
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
            DefaultRequestHeaders = {
                { "Accept", "application/json" }
            },
        });

        builder.Services.AddScoped(sp => new HttpClient()
        {
            //BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") ?? "localhost:5296"),
            
            
        });
        builder.Services.AddScoped<IInternalApiService, InternalApiService>();
        builder.Services.AddScoped<CookieDelegatingHandler>();
        builder.Services.AddScoped<UnauthorizedDelegatingHandler>();
        
        builder.Services.AddHttpClient("ServerAPI", 
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<CookieDelegatingHandler>()
            .AddHttpMessageHandler<UnauthorizedDelegatingHandler>();
        
        
        await builder.Build().RunAsync();
    }
}