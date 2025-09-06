using System.Collections.Concurrent;
using backend.Data.Services.Interfaces;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Shared.Data.Entities;
using Shared.Hubs.Interfaces;
using Shared.Utils;

namespace backend.Hubs;

public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    private readonly IChatMessageService  _chatMessageService;
    private readonly IRoomService _roomService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ChatHub> _logger;
    //private readonly ConcurrentDictionary<string, string> _groups;

    public ChatHub(IChatMessageService chatMessageService,
        UserManager<User> userManager,
        ILogger<ChatHub> logger, IRoomService roomService)
    {
        _chatMessageService = chatMessageService;
        _userManager = userManager;
        _logger = logger;
        _roomService = roomService;
    }


    public override async Task OnConnectedAsync()
    {

        var claims = Context.User;
        var user = await _userManager.GetUserAsync(claims);
        if (user == null)
        {
            return;
        }

        if (user.SignalrConnectionId is not null && user.SignalrConnectionId != Context.ConnectionId)
        {
            return;
        }
        
        
        await base.OnConnectedAsync();  
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var claims = Context.User;
        var user = await _userManager.GetUserAsync(claims);
        if (user == null)
        {
            return;
        }

        user.SignalrConnectionId = null;
        await _userManager.UpdateAsync(user);

        var roomsUserIsIn = await _roomService.GetAllRoomsUserIsIn(user);

        foreach (var room in roomsUserIsIn)
        {
            await RemoveFromGroup(room.Name);
            if (room.Owner.Id == user.Id)
            {
                await DeleteGroup(room.Name);
            }
        }

        //await Groups.RemoveFromGroupAsync(Context.ConnectionId, claims);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task<bool> AddToGroup(string groupName, string password)
    {
        password = HashUtil.Sha256String(password);
        var claims = Context.User;
        if (claims == null)
        {
            _logger.LogError("Claims are null, user is not logged in.");
            return false;
        }
        var user = await _userManager.GetUserAsync(claims);
        if (user == null)
        {
            _logger.LogError("User is not found.");
            return false;
        }
        user.SignalrConnectionId = Context.ConnectionId;
        await _userManager.UpdateAsync(user);
        
        var room = await _roomService.Read(groupName);
        if (room == null)
        {
            room = await _roomService.Create(new Room
            {
                Name = groupName,
                MaxAmountOfUser = 5,
                Owner = user,
                Password = password,
                Users = [user]
            });
        }
        else
        {
            if (room.Password != password)
            {
                _logger.LogError("Passwords do not match.");
                return false;
            }
        }

        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new ChatMessage
        {
            Group = groupName,
            Message = $"{user.UserName} has joined the group {groupName}."
        };
        await CreateChatMessage(sysMessage);
        await LoadMessages(groupName);
        return true;
    }

    public async Task RemoveFromGroup(string groupName)
    {
        //TODO: delete room if last user leaves or if they are the owner.
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new ChatMessage
        {
            Group = groupName,
            Message = $"{Context.User.Identity.Name} has left the group {groupName}."
        };
        await CreateChatMessage(sysMessage);
    }

    public async Task DeleteGroup(string groupName)
    {
        var room = await _roomService.Read(groupName);
        if (room == null)
        {
            return;
        }

        foreach (var user in room.Users)
        {
            if (user.SignalrConnectionId is null)
            {
                continue;
            }
            await Groups.RemoveFromGroupAsync(user.SignalrConnectionId, groupName);
        }

        var messages = await _chatMessageService.GetAllGroupChatMessages(groupName);
        await _chatMessageService.DeleteRange(messages);
        await _roomService.Delete(groupName);
    }

    public async Task LoadMessages(string groupName)
    {
        var messages = await _chatMessageService.GetAllGroupChatMessages(groupName);
        messages.Sort((x, y) => y.Timestamp.CompareTo(x.Timestamp) * -1);
        
        await Clients.Group(groupName).MessagesLoaded(messages);
    }

    public async Task CreateChatMessage(ChatMessage chatMessage)
    {
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).ChatMessageReceived(chatMessage);
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

        var chatMessage = new ChatMessage
        {
            Message = message,
            Group = group,
            Sender = user
        };
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).ChatMessageReceived(chatMessage);
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