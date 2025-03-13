using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Country
{
    //CountryId, Name
    [Key]
    public int CountryId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Town> Towns { get; set; } = new HashSet<Town>();
}