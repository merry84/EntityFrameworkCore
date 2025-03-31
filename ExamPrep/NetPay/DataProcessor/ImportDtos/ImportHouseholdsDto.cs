using NetPay.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ImportDtos;


[XmlType(nameof(Household))]
public class ImportHouseholdsDto
{
    [XmlElement("ContactPerson")]
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ContactPerson { get; set; } = null!;

    [XmlElement("Email")]
    [StringLength(80, MinimumLength = 6)]
    public string? Email { get; set; }

    [XmlAttribute("phone")]
    [Required]
    [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
    [StringLength(15, MinimumLength = 15)]
    public string PhoneNumber { get; set; } = null!;
}