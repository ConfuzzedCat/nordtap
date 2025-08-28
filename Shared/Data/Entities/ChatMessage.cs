using System.ComponentModel.DataAnnotations;

namespace Shared.Data.Entities;

public class ChatMessage : HubMessage
{
    public User Sender { get; set; }
    
    public ChatMessage()
    {
        
    }

    public override string ToString()
    {
        return $"{Sender.UserName}: {Message}";
    }
}