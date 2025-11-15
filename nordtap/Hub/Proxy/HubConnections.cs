using Microsoft.AspNetCore.SignalR.Client;
using Nordtap.Hub.Proxy;
using Shared.Hubs.Interfaces;

namespace frontend.Proxy;

public static class HubConnectionsExtensions
{
    // TODO: Make dynamic with source gens 
    public static void RegisterClient(this HubConnection hubConnection, IChatHubClient client)
    {
        var chatHubClientProxy = new ChatHubClientProxy();
        
        chatHubClientProxy.RegisterClient(hubConnection, client);
    }
}