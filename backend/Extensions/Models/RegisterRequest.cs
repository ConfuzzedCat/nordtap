namespace backend.Extensions.Models;

/// <summary>
/// The request type for the "/register" endpoint added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApiCustom"/>.
/// </summary>
public sealed class RegisterRequest
{
    /// <summary>
    /// The user's username.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }
        
    /// <summary>
    /// The user's invite code.
    /// </summary>
    public required Guid InviteCode { get; init; }
}