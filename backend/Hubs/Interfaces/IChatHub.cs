using Shared.Data.Entities;

namespace backend.Hubs.Interfaces;

public interface IChatHub
{
    Task<List<HubMessage>> LoadMessages();
    Task CreateChatMessage(ChatMessage chatMessage);
    Task CreateMessage(HubMessage message);
    Task ClearMessages();
    Task DeleteMessage(Guid id);
}