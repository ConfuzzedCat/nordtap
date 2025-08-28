using System.ComponentModel.DataAnnotations;

namespace Shared.Data.Entities;

public class HubMessage
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Group { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return Message;
    }
}