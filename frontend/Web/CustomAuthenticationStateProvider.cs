using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Data.Entities;
using Shared.Extensions.Models;

namespace frontend.Web;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "currentUser";

    private readonly ILocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetCurrentUserAsync();

        if(currentUser == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id!.ToString()!),
            new Claim(ClaimTypes.Name, currentUser.Username!.ToString()!),
        ];

        var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(CustomAuthenticationStateProvider))));

        return authenticationState;
    }

    public async Task SetCurrentUserAsync(InfoResponse? currentUser)
    { 
        await _localStorageService.SetItemAsync(LocalStorageKey, currentUser);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public ValueTask<InfoResponse?> GetCurrentUserAsync() => _localStorageService.GetItemAsync<InfoResponse>(LocalStorageKey);
    
    public ValueTask<bool> IsAuthenticatedAsync() => _localStorageService.ContainKeyAsync(LocalStorageKey);
}
