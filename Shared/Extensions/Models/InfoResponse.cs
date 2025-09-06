using Shared.Data.Entities;

namespace Shared.Extensions.Models;

/// <summary>
/// The response type for the "/manage/info" endpoints added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApiCustom"/>.
/// </summary>
public sealed class InfoResponse
{
    /// <summary>
    /// The username associated with the authenticated user.
    /// </summary>
    public required string Username { get; init; }
        
    public required string[] Roles { get; init; }
    
    public string Id {get; init;}
    
    public int GameWins { get; init; }
    public int GameLosses { get; init; }
    public int GameTies { get; init; }
    public int GameTotal { get; init; }
    public TimeSpan GameTime { get; init; }
    public DateTime CreationDate { get; init; }
    public Guid? InviteCode { get; init; }
    public bool isDeleted { get; init; }

    public InfoResponse()
    {
        
    }

    public InfoResponse(string username, string[] roles, string id, int gameWins, int gameLosses, int gameTies, int gameTotal, TimeSpan gameTime, DateTime creationDate, Guid? inviteCode, bool isDeleted)
    {
        Username = username;
        Roles = roles;
        Id = id;
        GameWins = gameWins;
        GameLosses = gameLosses;
        GameTies = gameTies;
        GameTotal = gameTotal;
        GameTime = gameTime;
        CreationDate = creationDate;
        InviteCode = inviteCode;
        this.isDeleted = isDeleted;
    }

    public static InfoResponse ToResponse(User user, string[] roles)
    {
        return new()
        {
            Username = user.UserName ?? "UsernameNotFound",
            GameWins = user.GameWins,
            GameLosses = user.GameLosses,
            GameTies = user.GameTies,
            GameTotal = user.GameTotal,
            GameTime = user.GameTime,
            CreationDate = user.CreationDate,
            InviteCode = user.InviteCode,
            isDeleted = user.IsDeleted,
            Id = user.Id,
            Roles = roles
        };

    }
}