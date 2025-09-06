using backend.Data.Context;
using backend.Data.Services.Interfaces;
using CSharpFunctionalExtensions;
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
    public async Task<Result<ChatMessage>> Create(ChatMessage chatMessage)
    {
        await _context.ChatMessagesDb.AddAsync(chatMessage);
        await _context.SaveChangesAsync();
        return chatMessage;
    }

    public async Task<Maybe<ChatMessage>> Read(Guid chatMessageId)
    {
        return await _context.ChatMessagesDb.FindAsync(chatMessageId);
    }

    public Task<Result<ChatMessage>> Update(ChatMessage chatMessage)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ChatMessage>> Delete(Guid chatMessageId)
    {
        var msg =  await Read(chatMessageId);
        if (msg.HasNoValue)
        {
            throw new NullReferenceException("Message was not found.");
        }
        var removedMsg = _context.ChatMessagesDb.Remove(msg.Value).Entity;
        await _context.SaveChangesAsync();
        return removedMsg;
    }

    public async Task<Result<List<ChatMessage>>> GetAllGroupChatMessages(string groupName)
    {
        return await _context.ChatMessagesDb
            .Where( msg => msg.Group == groupName)
            .Include(x => x.Sender)
            .ToListAsync();
    }

    public async Task<Result<List<ChatMessage>>> GetAllUserChatMessages(User user)
    {
        return await _context.ChatMessagesDb.Where( msg => msg.Sender == user).ToListAsync();
    }

    public async Task<Result<List<ChatMessage>>> GetAllUserGroupChatMessages(User user, string groupName)
    {
        return await _context.ChatMessagesDb.Where( msg => msg.Sender == user && msg.Group == groupName).ToListAsync();
    }

    public async Task<Result<List<ChatMessage>>> DeleteRange(List<Guid> messages)
    {
        var messagesToDelete = _context.ChatMessagesDb.Where(msg => messages.Contains(msg.Id));
        _context.ChatMessagesDb.RemoveRange(messagesToDelete);
        await _context.SaveChangesAsync();
        return await messagesToDelete.ToListAsync();
    }

    public async Task<Result<List<ChatMessage>>> DeleteRange(List<ChatMessage> messages)
    {
        _context.ChatMessagesDb.RemoveRange(messages);
        await _context.SaveChangesAsync();
        return messages;
    }
}