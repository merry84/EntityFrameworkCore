using System.Text.Json.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs;

public class ExportConversationDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    [JsonPropertyName("StartedAt")]
    public string StartedAt { get; set; }

    public List<ExportMessageDto> Messages { get; set; }
}