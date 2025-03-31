using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Data.Models;

public class UserConversation
{


    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [Required]
    public User User { get; set; }= null!;


    [ForeignKey(nameof(Conversation))]
    public int ConversationId { get; set; }
    [Required]
    public Conversation Conversation { get; set; } = null!;
}