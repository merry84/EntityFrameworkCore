using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models;

public class Supplier
{
    //Id – integer, Primary Key
    // SupplierName – text with length [3, 60] (required)
    // SuppliersServices - collection of type SupplierService

    [Key]
    public int Id  { get; set; }
    [Required]
    [StringLength(60, MinimumLength = 3)]//from dto

    public string SupplierName { get; set; } = null!;

    public virtual ICollection<SupplierService> SuppliersServices { get; set; } = new HashSet<SupplierService>();

}