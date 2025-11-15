using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Nordtap.LocalStorage;
using Shared.Extensions.Models;

namespace Nordtap.API;

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

    public ValueTask<InfoResponse?> GetCurrentUserAsync()
    {
        return _localStorageService.GetItemAsync<InfoResponse>(LocalStorageKey);
    }

    public ValueTask<bool> IsAuthenticatedAsync()
    {
        return _localStorageService.ContainKeyAsync(LocalStorageKey);
    }
}
