using backend.Data.Context;
using backend.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace backend.Data.Services;

public class HubMessageService : IHubMessageService
{
    private readonly DataContext _context;
    private readonly ILogger<HubMessageService> _logger;

    public HubMessageService(DataContext context, ILogger<HubMessageService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<HubMessage> Create(HubMessage hubMessage)
    {
        await _context.HubMessagesDb.AddAsync(hubMessage);
        await _context.SaveChangesAsync();
        return hubMessage;
    }

    public async Task<HubMessage?> Read(Guid hubMessageId)
    {
        return await _context.ChatMessagesDb.FindAsync(hubMessageId);
    }

    public Task<HubMessage> Update(HubMessage hubMessage)
    {
        throw new NotImplementedException();
    }

    public async Task<HubMessage> Delete(Guid hubMessageId)
    {
        var msg =  await Read(hubMessageId);
        if (msg == null)
        {
            throw new NullReferenceException("Message was not found.");
        }
        var removedMsg = _context.HubMessagesDb.Remove(msg).Entity;
        await _context.SaveChangesAsync();
        return removedMsg;
    }

    public async Task<List<HubMessage?>> GetAllGroupHubMessages(string groupName)
    {
        return await _context.HubMessagesDb.Select( msg => msg.Group == groupName ? msg : null).ToListAsync();
    }

    public async Task<List<HubMessage>> DeleteRange(List<Guid> messages)
    {
        var messagesToDelete = _context.HubMessagesDb.Where(msg => messages.Contains(msg.Id));
        _context.HubMessagesDb.RemoveRange(messagesToDelete);
        await _context.SaveChangesAsync();
        return await messagesToDelete.ToListAsync();
    }
}