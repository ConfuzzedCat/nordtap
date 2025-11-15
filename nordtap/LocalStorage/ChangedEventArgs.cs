namespace Nordtap.LocalStorage;

public class ChangedEventArgs
{
    public required string Key { get; set; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
}