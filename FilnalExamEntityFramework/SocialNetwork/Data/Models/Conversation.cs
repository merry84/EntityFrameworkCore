using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Data.Models;

public class Conversation
{
    //Id – integer, Primary Key
    // Title – text with length [2, 30] (required)
    // StartedAt – DateTime (required)
    // Messages – a collection of type Message
    // UsersConversations – a collection of type UserConversation

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime StartedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    public virtual ICollection<UserConversation> UsersConversations { get; set; } = new HashSet<UserConversation>();
}