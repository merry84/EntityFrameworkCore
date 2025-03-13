using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Color
{
    //ColorId, Name

    [Key] 
    public int ColorId { get; set; }

    [Required] 
    public string Name { get; set; } = null!;


    [InverseProperty(nameof(Team.PrimaryKitColor))]
    public ICollection<Team> PrimaryKitTeams { get; set; } = new HashSet<Team>();


    [InverseProperty(nameof(Team.SecondaryKitColor))]
    public ICollection<Team> SecondaryKitTeams { get; set; } = new HashSet<Team>();

}