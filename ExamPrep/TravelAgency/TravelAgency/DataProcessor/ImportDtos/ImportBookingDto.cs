using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TravelAgency.DataProcessor.ImportDtos;

public class ImportBookingDto
{
    [Required]
    [JsonProperty(nameof(BookingDate))]
    public string  BookingDate { get; set; } = null!;

    [Required]
    [JsonProperty(nameof(CustomerName))]
    public string CustomerName { get; set; } = null!;
    [Required]
    [JsonProperty(nameof(TourPackageName))]
    public string TourPackageName { get; set; } = null!;

    //     "BookingDate": "2024-09-21",
    //     "CustomerName": "Donald Sanders",
    //     "TourPackageName": "Horse Riding Tour"
}