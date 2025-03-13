using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Position
{
    //PositionId, Name
    [Key]
    public int  PositionId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Player> Players { get; set; } = new HashSet<Player>();   
}