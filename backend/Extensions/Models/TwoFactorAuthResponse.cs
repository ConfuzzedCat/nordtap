namespace backend.Extensions.Models;

public class TwoFactorAuthResponse
{
    public required string UnformattedKey { get; init; }
    public required string Username { get; init; }
    public required string[] RecoveryCodes { get; init; }
    public required bool FirstTimeEnabled { get; init; }
}