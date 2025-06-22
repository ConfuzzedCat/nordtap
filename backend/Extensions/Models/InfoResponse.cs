namespace backend.Extensions.Models;

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
    
    public required bool TwoFactorEnabled { get; init; }
}