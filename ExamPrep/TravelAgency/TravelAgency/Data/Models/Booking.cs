using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models;

public class Booking
{
    //Id – integer, Primary Key
    // BookingDate – DateTime (required)
    // CustomerId – integer, foreign key (required)
    // Customer – Customer
    // TourPackageId – integer, foreign key (required)
    // TourPackage – TourPackage
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }   
    [Required]
    [ForeignKey(nameof(Customer))]
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(TourPackage))]
    public int TourPackageId { get; set; }
    public virtual TourPackage TourPackage { get; set; } = null!;   

}