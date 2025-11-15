using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nordtap.API;

public class UnauthorizedDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<UnauthorizedDelegatingHandler> _logger;

    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

    public UnauthorizedDelegatingHandler(ILogger<UnauthorizedDelegatingHandler> logger, CustomAuthenticationStateProvider customAuthenticationStateProvider)
    {
        _logger = logger;
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var currentUser = await _customAuthenticationStateProvider.GetCurrentUserAsync();

            if(currentUser != null)
            {
                await _customAuthenticationStateProvider.SetCurrentUserAsync(null);
            }
        }

        return response;
    }
}