using Microsoft.AspNetCore.SignalR.Client;
using Shared.Data.Entities;
using Shared.Hubs.Infrastructure.Proxy;
using Shared.Hubs.Interfaces;

namespace frontend.Proxy;

public class ChatHubClientProxy : IHubClientProxy<IChatHubClient>
{
    private HubConnection _hubConnection;
    private IChatHubClient _client;

    public void RegisterClient(HubConnection hubConnection, IChatHubClient client)
    {
        _hubConnection = hubConnection;
        _client = client;

        _hubConnection.On<List<ChatMessage>>("MessagesLoaded", (messages) => _client.MessagesLoaded(messages));
        _hubConnection.On<Guid>("MessageDeleted", (id) => _client.MessageDeleted(id));
        //_hubConnection.On<HubMessage>("SystemMessageReceived", (message) => _client.SystemMessageReceived(message));
        _hubConnection.On<ChatMessage>("ChatMessageReceived", (message) => _client.ChatMessageReceived(message));
        _hubConnection.On<List<ChatMessage>>("MessagesDeleted", (messages) => _client.MessagesDeleted(messages));
    }
}