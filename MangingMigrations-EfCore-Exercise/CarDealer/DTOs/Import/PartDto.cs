﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Import;

public class PartDto
{
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("price")]
    public decimal Price { get; set; }

    [Required]
    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [Required]
    [JsonProperty("supplierId")]
    public int SupplierId { get; set; }
}