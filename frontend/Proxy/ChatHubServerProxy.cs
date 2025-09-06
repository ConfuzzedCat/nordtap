using CSharpFunctionalExtensions;
using frontend.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Data.DTO;
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

    public async Task<ResultDto> LoadMessages(string groupName)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("LoadMessages", groupName);
    }

    public async Task<ResultDto> CreateChatMessage(ChatMessage chatMessage)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("CreateChatMessage",chatMessage);
    }

    public async Task<ResultDto> CreateMessage(string group, string message)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("CreateMessage", group, message);
    }

    public async Task<ResultDto> ClearMessages(string groupName)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("ClearMessages", groupName);
    }

    public async Task<ResultDto> DeleteMessage(Guid id)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("DeleteMessage", id);
    }

    public async Task<ResultDto> AddToGroup(string groupName, string password)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("AddToGroup",groupName, password);
    }

    public async Task<ResultDto> RemoveFromGroup(string groupName)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("RemoveFromGroup", groupName);
    }

    public async Task<ResultDto> DeleteGroup(string groupName)
    {
        return await _hubConnection.InvokeAsync<ResultDto>("DeleteGroup", groupName);
    }
}