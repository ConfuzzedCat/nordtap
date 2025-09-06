using frontend.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Data.Entities;
using Shared.Hubs.Interfaces;

namespace frontend.Proxy;

public sealed class ChatHubServerProxy : IChatHub
{
    private readonly HubConnection _hubConnection;

    public ChatHubServerProxy(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public async Task LoadMessages(string groupName)
    {
        await _hubConnection.InvokeAsync("LoadMessages", groupName);
    }

    public async Task CreateChatMessage(ChatMessage chatMessage)
    {
        await _hubConnection.InvokeAsync("CreateChatMessage",chatMessage);
    }

    public async Task CreateMessage(string group, string message)
    {
        await _hubConnection.InvokeAsync("CreateMessage", group, message);
    }

    public async Task ClearMessages(string groupName)
    {
        await _hubConnection.InvokeAsync("ClearMessages", groupName);
    }

    public async Task DeleteMessage(Guid id)
    {
        await _hubConnection.InvokeAsync("DeleteMessage", id);
    }

    public async Task<bool> AddToGroup(string groupName, string password)
    {
        return await _hubConnection.InvokeAsync<bool>("AddToGroup",groupName, password);
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await _hubConnection.InvokeAsync("RemoveFromGroup", groupName);
    }

    public async Task DeleteGroup(string groupName)
    {
        await _hubConnection.InvokeAsync("DeleteGroup", groupName);
    }
}