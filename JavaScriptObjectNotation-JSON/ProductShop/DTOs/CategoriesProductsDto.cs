using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs;

public class CategoriesProductsDto
{
    [Required]
    [JsonProperty(nameof(ProductId))]
    public int ProductId { get; set; }
    [Required]
    [JsonProperty(nameof(CategoryId))]
    public int CategoryId { get; set; }
}