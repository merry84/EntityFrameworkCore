
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
     public class UserDto
    {
        [XmlElement("firstname")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastname")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ProductDtoExport[] SoldProducts { get; set; } = null!;
    }
}
