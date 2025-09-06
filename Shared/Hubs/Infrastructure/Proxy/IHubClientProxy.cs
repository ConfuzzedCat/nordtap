using Microsoft.AspNetCore.SignalR.Client;

namespace Shared.Hubs.Infrastructure.Proxy;

public interface IHubClientProxy<T>
{
    void RegisterClient(HubConnection connection, T client);
}