using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TravelAgency.DataProcessor.ImportDtos;

using System.ComponentModel.DataAnnotations;
[XmlType("Customer")]
public class ImportCustomerDto
{

    [Required]
    [StringLength(60, MinimumLength = 4)]
    [XmlElement("FullName")]
    public string FullName { get; set; } = null!;

    [XmlElement("Email")]
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Email { get; set; }= null!;

    [XmlAttribute("phoneNumber")]
    [Required]
    [RegularExpression(@"\+[0-9]{12}")]
    public string PhoneNumber { get; set; } = null!;



}
//Id – integer, Primary Key
// FullName – text with length [4, 60] (required)
// Email – text with length [6, 50] (required)
// PhoneNumber – text with length 13. (required)
// oAll phone numbers must have the following structure: a plus sign followed by 12 digits, without spaces or special characters: 
// Example -> +359888555444 
// HINT -> use DataAnnotation [RegularExpression] 
// Bookings - a collection of type Booking
