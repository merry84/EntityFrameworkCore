namespace CarDealer.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    public class CarDto
    {
        [Required] public int Id { get; set; }

        [Required][JsonProperty("make")] public string Make { get; set; } = null!;

        [Required][JsonProperty("model")] public string Model { get; set; } = null!;

        [Required]
        [JsonProperty("traveledDistance")]
        public long TraveledDistance { get; set; }

        [Required][JsonProperty("partsId")] public int[] partsId { get; set; } = null!;
    }
}

