using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IHubMessageService
{
    #region Default Crud methods
    Task<HubMessage> Create(HubMessage hubMessage);
    Task<HubMessage?> Read(Guid hubMessageId);
    Task<HubMessage> Update(HubMessage hubMessage);
    Task<HubMessage> Delete(Guid hubMessageId);
    #endregion
    
    Task<List<HubMessage?>> GetAllGroupHubMessages(string groupName);
    Task<List<HubMessage>> DeleteRange(List<Guid> messages);
}