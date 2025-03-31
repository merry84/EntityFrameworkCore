using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TravelAgency.DataProcessor.ExportDtos;

public class ExportBookingDto
{
    [Required]
    [JsonProperty(nameof(TourPackageName))]
    public string TourPackageName { get; set; } = null!;

    [Required]
    [JsonProperty(nameof(Date))]
    public string Date { get; set; } = null!;
    //"TourPackageName": "Horse Riding Tour",
    // "Date": "2024-09-21"
}