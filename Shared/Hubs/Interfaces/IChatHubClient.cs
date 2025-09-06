using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHubClient
{
    Task MessagesLoaded(List<ChatMessage> messages);
    //Task SystemMessageReceived(HubMessage message);
    Task ChatMessageReceived(ChatMessage message);
    Task MessageDeleted(Guid id);
    Task MessagesDeleted(List<ChatMessage> messages);
}