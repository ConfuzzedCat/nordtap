using System.ComponentModel.DataAnnotations;

namespace Shared.Data.Entities;

public class Room
{
    /// <summary>
    /// The name and identifier of the room
    /// </summary>
    [Key]
    public string Name { get; set; }
    
    /// <summary>
    /// A dictionary, where user is the key and connection id is the value, currently in the room. 
    /// </summary>
    public List<User> Users { get; set; }    
    
    /// <summary>
    /// This is a hash (sha256) version of the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The max amount of users allowed in the room.
    /// </summary>
    public uint MaxAmountOfUser { get; set; }

    /// <summary>
    /// The user that created the room/the owner of the room.
    /// </summary>
    public User Owner { get; set; }
}