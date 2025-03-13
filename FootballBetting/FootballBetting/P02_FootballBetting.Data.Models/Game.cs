using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    //GameId, HomeTeamId, AwayTeamId, HomeTeamGoals, AwayTeamGoals, HomeTeamBetRate, AwayTeamBetRate, DrawBetRate, DateTime, Result
    [Key]
    public int GameId { get; set; }


    [ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }

    public virtual Team HomeTeam { get; set; } = null!;



    [ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }
    public virtual Team AwayTeam { get; set; } = null!;

    public int HomeTeamGoals { get; set; }
    public int AwayTeamGoals { get; set; }

    public decimal HomeTeamBetRate { get; set; }
    public decimal AwayTeamBetRate { get; set; }
    public decimal DrawBetRate { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    public int Result { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = new HashSet<PlayerStatistic>();

    public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();

    public ICollection<Team> Teams { get; set; } = new HashSet<Team>();


}