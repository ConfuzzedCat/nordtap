using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHubClient
{
    Task MessageReceived(HubMessage message);
    Task MessageDeleted(Guid id);
}