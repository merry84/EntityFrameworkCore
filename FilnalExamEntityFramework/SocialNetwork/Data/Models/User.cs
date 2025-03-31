using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Data.Models;

public class User
{
    //Id – integer, Primary Key
    // Username – text with length [4, 20] (required)
    // Email – text with length [8, 60] (required)
    // Password – text with a minimum length of 6 (required)
    // Posts – a collection of type Post
    // Messages – a collection of type Message
    // UsersConversations – a collection of type UserConversation


    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(60, MinimumLength = 8)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    public virtual ICollection<UserConversation> UsersConversations { get; set; } 
        = new HashSet<UserConversation>();

}