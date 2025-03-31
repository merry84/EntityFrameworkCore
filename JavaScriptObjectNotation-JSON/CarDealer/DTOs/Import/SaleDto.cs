using System.ComponentModel.DataAnnotations;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class SaleDto
{
    [Required]
    [JsonProperty("discount")]
    public decimal Discount { get; set; }

    [Required]
    [JsonProperty("carId")]
    public int CarId { get; set; }

    [Required]
    [JsonProperty("customerId")]
    public int CustomerId { get; set; }
 
}