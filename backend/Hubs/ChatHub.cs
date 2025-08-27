using backend.Data.Services.Interfaces;
using backend.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Shared.Data.Entities;
using Shared.Hubs.Interfaces;

namespace backend.Hubs;

public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    private readonly IChatMessageServices  _chatMessageServices;

    public ChatHub(IChatMessageServices chatMessageServices)
    {
        _chatMessageServices = chatMessageServices;
    }


    public async Task SendMessage(string user, string message, string groupName)
    {
        
        await Clients.Group(groupName).MessageReceived()
        //await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }
    
    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        
        //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    public Task<List<HubMessage>> LoadMessages()
    {
        throw new NotImplementedException();
    }

    public Task CreateChatMessage(ChatMessage chatMessage)
    {
        throw new NotImplementedException();
    }

    public Task CreateMessage(HubMessage message)
    {
        throw new NotImplementedException();
    }

    public Task ClearMessages()
    {
        throw new NotImplementedException();
    }

    public Task DeleteMessage(Guid id)
    {
        throw new NotImplementedException();
    }
}