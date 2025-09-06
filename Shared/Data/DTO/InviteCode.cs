namespace Shared.Data.DTO;

public class InviteCode
{
    public Guid Code { get; set; }
    public bool IsUsed { get; set; }
    public Guid? IssuerId { get; set; }
}