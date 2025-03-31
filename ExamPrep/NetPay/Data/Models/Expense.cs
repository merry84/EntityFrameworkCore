 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetPay.Data.Models.Enums;

namespace NetPay.Data.Models;

public class Expense
{
 

    [Key]
    public int  Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ExpenseName { get; set; } = null!;

    [Required]
    [Range(0.01, 100000)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; }

    [Required]
    [ForeignKey(nameof(Household))]
    public int HouseholdId { get; set; }
    public Household Household { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Service))]
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;

}