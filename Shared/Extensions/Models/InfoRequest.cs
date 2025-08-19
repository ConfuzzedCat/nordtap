namespace Shared.Extensions.Models;

/// <summary>
/// The request type for the "/manage/info" endpoint added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApiCustom"/>.
/// All properties are optional. No modifications will be made to the user if all the properties are omitted from the request.
/// </summary>
public sealed class InfoRequest
{
    /// <summary>
    /// The optional new username for the authenticated user. This will replace the username if there was one.
    /// </summary>
    public string? NewUsername { get; init; }

    /// <summary>
    /// The optional new password for the authenticated user. If a new password is provided, the <see cref="OldPassword"/> is required.
    /// If the user forgot the old password, use the "/forgotPassword" endpoint instead.
    /// </summary>
    public string? NewPassword { get; init; }

    /// <summary>
    /// The old password for the authenticated user. This is only required if a <see cref="NewPassword"/> is provided.
    /// </summary>
    public string? OldPassword { get; init; }
}