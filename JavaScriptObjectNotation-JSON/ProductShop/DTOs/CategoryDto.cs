using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs;

public class CategoryDto
{
    [Required]
    [JsonProperty("name")]
    public string? Name { get; set; }
}