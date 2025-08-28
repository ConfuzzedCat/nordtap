using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IChatMessageService
{

    #region Default Crud methods
    Task<ChatMessage> Create(ChatMessage chatMessage);
    Task<ChatMessage?> Read(Guid chatMessageId);
    Task<ChatMessage> Update(ChatMessage chatMessage);
    Task<ChatMessage> Delete(Guid chatMessageId);
    #endregion
    
    Task<List<ChatMessage?>> GetAllGroupChatMessages(string groupName);
    Task<List<ChatMessage?>> GetAllUserChatMessages(User user);
    Task<List<ChatMessage?>> GetAllUserGroupChatMessages(User user, string groupName);
    Task<List<ChatMessage>> DeleteRange(List<Guid> messages);
}