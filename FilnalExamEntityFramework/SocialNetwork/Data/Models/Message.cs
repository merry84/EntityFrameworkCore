using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocialNetwork.Data.Enums;

namespace SocialNetwork.Data.Models;

public class Message
{
    //Id – integer, Primary Key
    // Content – text with length 1, 200] (required)
    // SentAt – DateTime (required)
    // Status – enum MessageStatus(Sent = 0, Delivered, Seen, Failed)(required)
    // ConversationId - integer, Foreign Key (required)
    // Conversation - Conversation
    // SenderId - integer, Foreign Key (required)
    // Sender – User

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime SentAt { get; set; }
    [Required]
    public MessageStatus Status { get; set; }

    [Required]
    [ForeignKey(nameof(Conversation))]
    public int ConversationId { get; set; }
    public virtual Conversation Conversation { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Sender))]
    public int SenderId { get; set; }
    public virtual User Sender { get; set; } = null!;

}