using CSharpFunctionalExtensions;
using Shared.Data.DTO;
using Shared.Data.Entities;

namespace Shared.Hubs.Interfaces;

public interface IChatHub
{
    Task<ResultDto> LoadMessages(string groupName);
    Task<ResultDto> CreateChatMessage(ChatMessage chatMessage);
    //Task ResulTOtDteSystemMessage(HubMessage message);
    Task<ResultDto> CreateMessage(string group, string message);
    Task<ResultDto> ClearMessages(string groupName);
    Task<ResultDto> DeleteMessage(Guid id);
    Task<ResultDto> AddToGroup(string groupName, string password);
    Task<ResultDto> RemoveFromGroup(string groupName);
    Task<ResultDto> DeleteGroup(string groupName);
}