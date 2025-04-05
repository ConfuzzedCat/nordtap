using System.Net;
using frontend.Components;
using frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace frontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddScoped(sp => new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        })
        {
            //BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl") ?? "localhost:5296"),
            DefaultRequestHeaders = { 
                { "Accept", "application/json" }
            },
            
        });
        builder.Services.AddScoped<IInternalApiService, InternalApiService>();
        builder.Services.AddHttpContextAccessor();

        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}