using System.Text.Json.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs;

public class ExportMessageDto
{
    public string Content { get; set; }

    [JsonPropertyName("SentAt")]
    public string SentAt { get; set; }

    public int Status { get; set; }

    public string SenderUsername { get; set; }
}