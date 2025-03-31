

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("CategoryProduct")]
    public class CategoryProductsDto
    {
        [XmlElement("ProductId")]
        [Required]
        public string ProductId { get; set; }= null!;

        [XmlElement("CategoryId")]
        [Required]
        public string CategoryId { get; set; } = null!;
    }
}
