using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TravelAgency.DataProcessor.ExportDtos;

public class ExportCustomerDto
{
    [Required]
    [JsonProperty("FullName")]
    public string FullName { get; set; } = null!;

    [Required]
    [JsonProperty("PhoneNumber")]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    [JsonProperty("Bookings")]
    public ExportBookingDto[] Bookings { get; set; } = null!;

   
}