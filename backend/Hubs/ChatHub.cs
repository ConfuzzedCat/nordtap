using System.Collections.Concurrent;
using backend.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Shared.Data.Entities;
using Shared.Hubs.Interfaces;

namespace backend.Hubs;

public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    private readonly IChatMessageService  _chatMessageService;
    private readonly IHubMessageService  _hubMessageService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ChatHub> _logger;
    private readonly ConcurrentDictionary<string, string> _groups;

    public ChatHub(IChatMessageService chatMessageService,
        IHubMessageService hubMessageService,
        UserManager<User> userManager,
        ILogger<ChatHub> logger)
    {
        _chatMessageService = chatMessageService;
        _hubMessageService = hubMessageService;
        _userManager = userManager;
        _logger = logger;
        _groups = [];
    }
    
    public async Task<bool> AddToGroup(string groupName, string passwordHash)
    {
        var created = _groups.TryAdd(groupName, passwordHash);
        if (created == false)
        {
            if (_groups[groupName] != passwordHash)
            {
                _logger.LogInformation("Wrong password for {GroupName}.", groupName);
                return false;
            }
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new HubMessage
        {
            Group = groupName,
            Message = $"{Context.UserIdentifier} has joined the group {groupName}."
        };
        await CreateMessage(sysMessage);
        return true;
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new HubMessage
        {
            Group = groupName,
            Message = $"{Context.UserIdentifier} has left the group {groupName}."
        };
        await CreateMessage(sysMessage);
    }

    public async Task LoadMessages(string groupName)
    {
        var chatMessages = await _chatMessageService.GetAllGroupChatMessages(groupName);
        var hubMessages = await _hubMessageService.GetAllGroupHubMessages(groupName);
        var messages = chatMessages.Concat(hubMessages).OrderByDescending(msg => msg.Timestamp);
        
        await Clients.Group(groupName).MessagesLoaded(messages.ToList());
    }

    public async Task CreateChatMessage(ChatMessage chatMessage)
    {
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).MessageReceived(chatMessage);
    }

    public async Task CreateMessage(HubMessage message)
    {
        await _hubMessageService.Create(message);
        await Clients.Group(message.Group).MessageReceived(message);
    }

    public async Task CreateMessage(string group, string message)
    {
        var claimsPrincipal = Context.User;
        
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("User is not logged in, aborting.");
            return;
        }

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            _logger.LogWarning("User is null, aborting.");
            return;
        }

        var chatMessage = new ChatMessage()
        {
            Message = message,
            Group = "",
            Sender = user
        };
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).MessageReceived(chatMessage);
    }

    public async Task ClearMessages(string groupName)
    {
        var ids = (await _chatMessageService.GetAllGroupChatMessages(groupName)).Select(msg => msg.Id);
        var guids = ids as Guid[] ?? ids.ToArray();
        await _chatMessageService.DeleteRange(guids.ToList());
        await Clients.Group(groupName).MessagesDeleted(guids);
    }

    public async Task DeleteMessage(Guid id)
    {
        var chatMessage = await _chatMessageService.Read(id);
        await _chatMessageService.Delete(id);
        await Clients.Group(chatMessage.Group).MessageDeleted(id);
    }
}