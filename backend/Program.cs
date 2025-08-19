using backend.Data;
using backend.Data.Context;
using backend.Data.Services;
using backend.Data.Services.Interfaces;
using backend.Extensions;
using backend.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.OpenApi.Models;
using Shared.Data.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace backend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IInviteCodeService, InviteCodeService>();
        builder.Services.AddSignalR();
        builder.Services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                [ "application/octet-stream" ]);
        });

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        var connectionString = builder.Configuration["db:CONNECTION_STRING"] ?? builder.Configuration.GetConnectionString("Dev");
        builder.Services.AddDbContext<DataContext>(opt =>
            opt.UseNpgsql(connectionString));

        builder.Services.AddAuthorization();
        builder.Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();
        
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Default User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ0123456789-._@+";
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "NordtapCookie";
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
        });
        
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("BlazorWASM", policyBuilder =>
            {
                policyBuilder.WithOrigins("https://localhost:7151","http://localhost:5247")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseCors("BlazorWASM");

        app.UseAuthorization();
        app.UseResponseCompression();

        app.MapControllers();
        app.MapIdentityApiCustom();
        app.MapHub<ChatHub>("/ChatHub");
        
        
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Constants.Roles)
            {
                if (await roleManager.RoleExistsAsync(role)) continue;
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded) throw new Exception($"Couldn't create role: {role}");
            }
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var inviteCodeService = scope.ServiceProvider.GetRequiredService<IInviteCodeService>();
            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<User>>();
            
            
            var username =  builder.Configuration["defaults:users:cat:username"];
            var password = builder.Configuration["defaults:users:cat:password"];

            if (userManager.Users.Any(u => u.UserName == username) == false)
            {
                var invCode = (await inviteCodeService.GenerateNewCode(null)).Code;
                var user = new User(invCode);
                var isSetToUsed = await inviteCodeService.SetCodeStatus(invCode);
                if (isSetToUsed == false)
                {
                    throw new Exception($"Couldn't set invite code as used. Code: {invCode}");
                }
                await userStore.SetUserNameAsync(user, username, CancellationToken.None);
                var result = await userManager.CreateAsync(user, password!);
                if (result.Succeeded == false)
                {
                    var errors = result.Errors.Select((err) => $"{err.Code}: {err.Description}");
                    throw new Exception($"Couldn't create user: {user}\n{string.Join(Environment.NewLine, errors)}");
                }

                var isInRoleAsync = await userManager.IsInRoleAsync(user, "admin");
                if (isInRoleAsync == false)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }

        await app.RunAsync();
    }
}