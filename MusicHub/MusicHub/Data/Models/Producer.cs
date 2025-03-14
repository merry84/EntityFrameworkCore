using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Producer
{

    [Key]
    public int  Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;


    public string? Pseudonym { get; set; }


    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new HashSet<Album>();

}