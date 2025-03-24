using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class CustomerDto
{
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("birthDate")]
    public DateTime BirthDate { get; set; }

    [Required]
    [JsonProperty("isYoungDriver")]
    public bool IsYoungDriver { get; set; }
}