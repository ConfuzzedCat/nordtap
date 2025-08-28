using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHub
{
    Task LoadMessages(string groupName);
    Task CreateChatMessage(ChatMessage chatMessage);
    Task CreateMessage(HubMessage message);
    Task CreateMessage(string group, string message);
    Task ClearMessages(string groupName);
    Task DeleteMessage(Guid id);
    Task<bool> AddToGroup(string groupName, string passwordHash);
    Task RemoveFromGroup(string groupName);

}