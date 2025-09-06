using System.Collections.Concurrent;
using backend.Data.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Shared.Data.DTO;
using Shared.Data.Entities;
using Shared.Extensions;
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

    public override async Task<Result> OnDisconnectedAsync(Exception? exception)
    {
        var claims = Context.User;
        if (claims == null)
        {
            return Result.Failure("User not logged in.");
        }
        var user = await _userManager.GetUserAsync(claims);
        if (user == null)
        {
            return Result.Failure("User not found.");
        }

        user.SignalrConnectionId = null;
        await _userManager.UpdateAsync(user);

        var roomsUserIsIn = await _roomService.GetAllRoomsUserIsIn(user);

        if (roomsUserIsIn.IsFailure)
        {
            return Result.Failure("User is not in any room");
        }
        
        foreach (var room in roomsUserIsIn.Value)
        {
            await RemoveFromGroup(room.Name);
            if (room.Owner.Id == user.Id)
            {
                await DeleteGroup(room.Name);
            }
        }

        //await Groups.RemoveFromGroupAsync(Context.ConnectionId, claims);

        await base.OnDisconnectedAsync(exception);
        return Result.Success();
    }

    public async Task<ResultDto> AddToGroup(string groupName, string password)
    {
        password = HashUtil.Sha256String(password);
        var claims = Context.User;
        if (claims == null)
        {
            const string failure = "Claims are null, user is not logged in.";
            _logger.LogError(failure);
            return Result.Failure(failure).ToDto();
        }
        var user = await _userManager.GetUserAsync(claims);
        if (user == null)
        {
            const string failure = "User is not found.";
            _logger.LogError(failure);
            return Result.Failure(failure).ToDto();
        }
        user.SignalrConnectionId = Context.ConnectionId;
        await _userManager.UpdateAsync(user);
        
        Room room;
        var roomMaybe = await _roomService.Read(groupName);
        bool isRoomNew = false;
        if (roomMaybe.HasNoValue)
        {
            room = (await _roomService.Create(new Room
            {
                Name = groupName,
                MaxAmountOfUser = 5,
                Owner = user,
                Password = password,
                Users = [user]
            })).Value;
            isRoomNew = true;
        }
        else
        {
            room = roomMaybe.Value;
            if (room.Password != password)
            {
                _logger.LogWarning("User '{user}' tried to join room '{room}', but passwords do not match.", user.UserName, room.Name);
                return Result.Failure(
                    $"User '{user.UserName}' tried to join room '{room.Name}', but passwords do not match.").ToDto();
            }
        }

        if (room.Owner.Id == user.Id && isRoomNew is false)
        {
            const string failure = "User is the owner of the room.";
            _logger.LogWarning(failure);
            return Result.Failure(failure).ToDto();
        }

        if (room.Users.Contains(user) && isRoomNew is false)
        {
            const string failure = "User is already in the room.";
            _logger.LogWarning(failure);
            return Result.Failure(failure).ToDto();
        }

        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new ChatMessage
        {
            Group = groupName,
            Message = $"{user.UserName} has joined the group {groupName}."
        };
        await CreateChatMessage(sysMessage);
        await LoadMessages(groupName);
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> RemoveFromGroup(string groupName)
    {
        //TODO: delete room if last user leaves or if they are the owner.
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        var sysMessage = new ChatMessage
        {
            Group = groupName,
            Message = $"{Context.User.Identity.Name} has left the group {groupName}."
        };
        return await CreateChatMessage(sysMessage);
    }

    public async Task<ResultDto> DeleteGroup(string groupName)
    {
        var room = await _roomService.Read(groupName);
        if (room.HasNoValue)
        {
            return  Result.Failure("Room not found.").ToDto();
        }

        foreach (var user in room.Value.Users)
        {
            if (user.SignalrConnectionId is null)
            {
                continue;
            }
            await Groups.RemoveFromGroupAsync(user.SignalrConnectionId, groupName);
        }

        var messagesResult = await _chatMessageService.GetAllGroupChatMessages(groupName);
        if (messagesResult.IsFailure)
        {
            return Result.Failure(messagesResult.Error).ToDto();
        }
        var messages = messagesResult.Value;
        await _chatMessageService.DeleteRange(messages);
        var didRoomDelete = await _roomService.Delete(groupName);
        //return Result.SuccessIf(didRoomDelete.IsSuccess, "Room was not deleted.");
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> LoadMessages(string groupName)
    {
        var messagesResult = await _chatMessageService.GetAllGroupChatMessages(groupName);
        if (messagesResult.IsFailure)
        {
            return Result.Failure(messagesResult.Error).ToDto();
        }
        var messages = messagesResult.Value;
        messages.Sort((x, y) => y.Timestamp.CompareTo(x.Timestamp) * -1);
        
        await Clients.Group(groupName).MessagesLoaded(messages);
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> CreateChatMessage(ChatMessage chatMessage)
    {
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).ChatMessageReceived(chatMessage);
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> CreateMessage(string group, string message)
    {
        var claimsPrincipal = Context.User;
        
        if (claimsPrincipal == null)
        {
            const string failure = "Claims are null, user is not logged in.";
            _logger.LogWarning(failure);
            return Result.Failure(failure).ToDto();
        }

        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            const string failure = "User is not found.";
            _logger.LogWarning(failure);
            return Result.Failure(failure).ToDto();
        }

        var chatMessage = new ChatMessage
        {
            Message = message,
            Group = group,
            Sender = user
        };
        await _chatMessageService.Create(chatMessage);
        await Clients.Group(chatMessage.Group).ChatMessageReceived(chatMessage);
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> ClearMessages(string groupName)
    {
        var messagesResult = await _chatMessageService.GetAllGroupChatMessages(groupName);
        if (messagesResult.IsFailure)
        {
            return Result.Failure(messagesResult.Error).ToDto();
        }
        var messages = messagesResult.Value;
        await _chatMessageService.DeleteRange(messages);
        await Clients.Group(groupName).MessagesDeleted(messages);
        return Result.Success().ToDto();
    }

    public async Task<ResultDto> DeleteMessage(Guid id)
    {
        var chatMessageResult = await _chatMessageService.Read(id);
        if (chatMessageResult.HasNoValue)
        {
            return Result.Failure("Couldn't find message").ToDto();
        }
        var chatMessage = chatMessageResult.Value;
        var deleteResult = await _chatMessageService.Delete(id);
        if (deleteResult.IsFailure)
        {
            return Result.Failure(deleteResult.Error).ToDto();
        }
        await Clients.Group(chatMessage.Group).MessageDeleted(id);
        return Result.Success().ToDto();
    }
}