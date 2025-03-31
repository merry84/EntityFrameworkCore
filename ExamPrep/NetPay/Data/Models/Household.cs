using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace NetPay.Data.Models;

public class Household
{
    //Id – integer, Primary Key
    // ContactPerson - text with length [5, 50] (required)
    // Email – text with length [6, 80] (not required)
    // PhoneNumber – text with length 15. (required)
    // oThe phone number must start with a plus sign, followed by exactly three digits for the country code,
    // a slash, exactly three digits for the area or service provider code,
    // a dash, and exactly six digits for the subscriber number: 
    // Example -> +144/123-123456 
    // Use the following string for correct validation: @"^\+\d{3}/\d{3}-\d{6}$"
    // Expenses - a collection of type Expense

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]

    public string ContactPerson { get; set; }=null!;
    [MaxLength(80)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(15)]
    [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]//from dto
    public string PhoneNumber { get; set; }= null!;
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
}