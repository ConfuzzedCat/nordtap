using CSharpFunctionalExtensions;
using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IChatMessageService : ICrud<Guid, ChatMessage>
{
    Task<Result<List<ChatMessage>>> GetAllGroupChatMessages(string groupName);
    Task<Result<List<ChatMessage>>> GetAllUserChatMessages(User user);
    Task<Result<List<ChatMessage>>> GetAllUserGroupChatMessages(User user, string groupName);
    Task<Result<List<ChatMessage>>> DeleteRange(List<Guid> messages);
    Task<Result<List<ChatMessage>>> DeleteRange(List<ChatMessage> messages);
}