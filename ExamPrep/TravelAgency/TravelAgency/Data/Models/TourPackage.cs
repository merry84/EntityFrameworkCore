using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models;

public class TourPackage
{
    //Id
    // PackageName – text with length [2, 40] (required)
    // Description – text with max length 200 (not required)
    // Price – a positive decimal value (required)
    // Bookings - a collection of type Booking
    // TourPackagesGuides - collection of type TourPackageGuide

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string PackageName { get; set; }= null!;

    [MaxLength(200)]
    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

    public ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();
}