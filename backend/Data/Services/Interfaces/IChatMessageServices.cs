using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IChatMessageServices
{

    #region Default Crud methods
    Task<ChatMessage> Create(ChatMessage chatMessage);
    Task<ChatMessage> ReadAsync(Guid chatMessageId);
    Task<ChatMessage> Update(ChatMessage chatMessage);
    Task<ChatMessage> Delete(Guid chatMessageId);
    #endregion
    
    Task<List<ChatMessage>> GetAllGroupChatMessages(string groupName);
    Task<List<ChatMessage>> GetAllUserChatMessages(User user);
    Task<List<ChatMessage>> GetAllUserGroupChatMessages(User user, string groupName);
}