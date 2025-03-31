using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Data.Models;

public class Post
{
    //Id – integer, Primary Key
    // Content – text with length [5, 300] (required)
    // CreatedAt – DateTime (required)
    // CreatorId – integer, Foreign Key (required)
    // Creator – User
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 5)]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [ForeignKey(nameof(Creator))]
    public int CreatorId { get; set; }
    public virtual User Creator { get; set; } = null!;



}