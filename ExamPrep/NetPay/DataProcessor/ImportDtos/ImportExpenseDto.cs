using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NetPay.Data.Models.Enums;
using Newtonsoft.Json;

namespace NetPay.DataProcessor.ImportDtos;

public class ImportExpenseDto
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    [JsonProperty("ExpenseName")]
    public string ExpenseName { get; set; } = null!;

    [Required]
    [Range(0.01, 100000)]
    [JsonProperty("Amount")]
    public decimal Amount { get; set; }


    [Required]
    [JsonProperty("DueDate")]
    public string DueDate { get; set; } = null!;

    [Required]
    [JsonProperty("PaymentStatus")]
    [EnumDataType(typeof(PaymentStatus))]
    public string PaymentStatus { get; set; } = null!;

    [Required]
    [JsonProperty("HouseholdId")]
    public int HouseholdId { get; set; }
    [Required]
    [JsonProperty("ServiceId")]
    public int ServiceId { get; set; }
    
}