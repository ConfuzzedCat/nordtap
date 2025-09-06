using System.ComponentModel.DataAnnotations;

namespace Shared.Data.Entities;

public class ChatMessage 
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Group { get; set; }
    public string Message { get; set; }
    public User? Sender { get; set; }
    
    public ChatMessage()
    {
        
    }

    public override string ToString()
    {
        return Sender == null ? Message : $"{Sender.UserName}: {Message}";
    }
}