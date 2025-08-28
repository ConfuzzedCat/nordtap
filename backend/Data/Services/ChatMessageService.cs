using backend.Data.Context;
using backend.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace backend.Data.Services;

public class ChatMessageService : IChatMessageService
{
    private readonly DataContext _context;
    private readonly ILogger<ChatMessageService> _logger;
    
    public ChatMessageService(DataContext context, ILogger<ChatMessageService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ChatMessage> Create(ChatMessage chatMessage)
    {
        await _context.ChatMessagesDb.AddAsync(chatMessage);
        await _context.SaveChangesAsync();
        return chatMessage;
    }

    public async Task<ChatMessage?> Read(Guid chatMessageId)
    {
        return await _context.ChatMessagesDb.FindAsync(chatMessageId);
    }

    public Task<ChatMessage> Update(ChatMessage chatMessage)
    {
        throw new NotImplementedException();
    }

    public async Task<ChatMessage> Delete(Guid chatMessageId)
    {
        var msg =  await Read(chatMessageId);
        if (msg == null)
        {
            throw new NullReferenceException("Message was not found.");
        }
        var removedMsg = _context.ChatMessagesDb.Remove(msg).Entity;
        await _context.SaveChangesAsync();
        return removedMsg;
    }

    public async Task<List<ChatMessage?>> GetAllGroupChatMessages(string groupName)
    {
        return await _context.ChatMessagesDb.Select( msg => msg.Group == groupName ? msg : null).ToListAsync();
    }

    public async Task<List<ChatMessage?>> GetAllUserChatMessages(User user)
    {
        return await _context.ChatMessagesDb.Select( msg => msg.Sender == user ? msg : null).ToListAsync();
    }

    public async Task<List<ChatMessage?>> GetAllUserGroupChatMessages(User user, string groupName)
    {
        return await _context.ChatMessagesDb.Select( msg => (msg.Sender == user && msg.Group == groupName) ? msg : null).ToListAsync();
    }

    public async Task<List<ChatMessage>> DeleteRange(List<Guid> messages)
    {
        var messagesToDelete = _context.ChatMessagesDb.Where(msg => messages.Contains(msg.Id));
        _context.ChatMessagesDb.RemoveRange(messagesToDelete);
        await _context.SaveChangesAsync();
        return await messagesToDelete.ToListAsync();
    }
}