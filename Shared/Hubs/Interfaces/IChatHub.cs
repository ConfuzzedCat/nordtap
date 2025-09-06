using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHub
{
    Task LoadMessages(string groupName);
    Task CreateChatMessage(ChatMessage chatMessage);
    //Task CreateSystemMessage(HubMessage message);
    Task CreateMessage(string group, string message);
    Task ClearMessages(string groupName);
    Task DeleteMessage(Guid id);
    Task<bool> AddToGroup(string groupName, string password);
    Task RemoveFromGroup(string groupName);
    Task DeleteGroup(string groupName);
}