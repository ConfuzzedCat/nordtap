namespace Shared.Extensions.Models;

/// <summary>
/// The request type for the "/login" endpoint added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApiCustom"/>.
/// </summary>
public sealed class LoginRequest
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
    /// Whether to remember the user.
    /// </summary>
    public required bool RememberMe { get; init; }
}