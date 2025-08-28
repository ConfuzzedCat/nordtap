using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHubClient
{
    Task MessagesLoaded(List<HubMessage> messages);
    Task MessageReceived(HubMessage message);
    Task MessageDeleted(Guid id);
    Task MessagesDeleted(IEnumerable<Guid> ids);
}