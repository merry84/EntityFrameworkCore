using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class User
{
    //UserId, Username, Name, Password, Email, Balance
    [Key]
    public int  UserId { get; set; }


    [Required] 
    public string Username { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;


    [Required]
    public string Password { get; set; } = null!;


    [Required]
    public string Email { get; set; } = null!;

    public decimal Balance { get; set; }

    public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();


}