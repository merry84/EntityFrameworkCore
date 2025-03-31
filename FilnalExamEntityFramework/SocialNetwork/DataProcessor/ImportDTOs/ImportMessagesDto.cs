using SocialNetwork.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ImportDTOs;


[XmlType(nameof(Message))]
public class ImportMessagesDto
{
    
    [Required]
    [XmlElement(nameof(Content))]
    [StringLength(200, MinimumLength = 1)]
    public string Content { get; set; } = null!;

    [Required]
    [XmlAttribute(nameof(SentAt))]
    public string SentAt { get; set; } = null!;

    [Required]
    [XmlElement(nameof(Status))]
    public string Status { get; set; } = null!;

    [Required]
    [XmlElement(nameof(ConversationId))]
    public int ConversationId { get; set; }
    
    [Required]
    [XmlElement(nameof(SenderId))]
    public int SenderId { get; set; }
    



}