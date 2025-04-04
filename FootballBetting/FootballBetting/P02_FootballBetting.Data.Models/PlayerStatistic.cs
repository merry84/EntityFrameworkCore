﻿using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class PlayerStatistic
{
    //GameId, PlayerId, ScoredGoals, Assists, MinutesPlayed


    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }

    public Game Game { get; set; } = null!;


    [ForeignKey(nameof(Player))]
    public int  PlayerId { get; set; }

    public Player Player { get; set; } = null!;


    public int ScoredGoals { get; set; }

    public int Assists { get; set; }
    public int MinutesPlayed { get; set; }
   

}