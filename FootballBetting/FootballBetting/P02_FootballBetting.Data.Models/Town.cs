using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    //TownId, Name, CountryId
    [Key]
    public int TownId { get; set; }

    [Required]
    public string Name { get; set; } = null!;


    [Required]
    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    public ICollection<Player> Players { get; set; } = new HashSet<Player>();
}