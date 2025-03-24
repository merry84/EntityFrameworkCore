

namespace ProductShop.DTOs
{
    using System.ComponentModel.DataAnnotations;
            using Newtonsoft.Json;
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int SellerId { get; set; }

        public int? BuyerId { get; set; }


    }
}


