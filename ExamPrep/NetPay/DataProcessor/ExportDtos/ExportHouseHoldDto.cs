using System.ComponentModel.DataAnnotations;
using NetPay.Data.Models;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos;
[XmlType(nameof(Household))]
public class ExportHouseHoldDto
{
    [XmlElement("Name")]
    [StringLength(50, MinimumLength = 5)]
    public string ContactPerson { get; set; } = null!;

    [XmlElement("Email")]
    [StringLength(80, MinimumLength = 6)]
    public string? Email { get; set; }

    [XmlElement("Phone")]
    [StringLength(15)]
    public string PhoneNumber { get; set; } = null!;

    [XmlArray("Expenses")]
    [XmlArrayItem("Expense")]
    public ExportExpenseDto[] Expenses { get; set; } = null!;
    
}