using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models;

public class Customer
{
    //Id – integer, Primary Key
    // FullName – text with length [4, 60] (required)
    // Email – text with length [6, 50] (required)
    // PhoneNumber – text with length 13. (required)
    // oAll phone numbers must have the following structure: a plus sign followed by 12 digits, without spaces or special characters: 
    // Example -> +359888555444 
    // HINT -> use DataAnnotation [RegularExpression] 
    // Bookings - a collection of type Booking
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(60)]
    public string FullName { get; set; }=null!;

    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(13)]
    [RegularExpression(@"\+[0-9]{12}")]
    public string PhoneNumber { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
}