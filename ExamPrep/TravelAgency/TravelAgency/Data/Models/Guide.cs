using System.ComponentModel.DataAnnotations;
using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data.Models;

public class Guide
{
    //Id – integer, Primary Key
    // FullName – text with length [4, 60] (required)
    // Language – Language enum (English = 0, German, French, Spanish, Russian) (required)
    // TourPackagesGuides - collection of type TourPackageGuide

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string FullName { get; set; } = null!;

    [Required]
    public Language Language { get; set; }
    public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new HashSet<TourPackageGuide>();

}