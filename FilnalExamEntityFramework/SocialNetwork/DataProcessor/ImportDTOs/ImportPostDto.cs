using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SocialNetwork.DataProcessor.ImportDTOs;

public class ImportPostDto
{
    [Required]
    [JsonProperty(nameof(Content))]
    [StringLength(300, MinimumLength = 5)]
    public string Content { get; set; } = null!;

    [Required]
    [JsonProperty(nameof(CreatedAt))]
    public string CreatedAt { get; set; }= null!;

    [Required]
    [JsonProperty(nameof(CreatorId))]
    public int CreatorId { get; set; }
}