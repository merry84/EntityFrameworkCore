using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Data.Models.Enums;
namespace MusicHub.Data.Models;

public class Song
{

    [Key]
    public int Id { get; set; }

    [Required] [MaxLength(20)] public string Name { get; set; } = null!;

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Genre Genre { get; set; }

    [ForeignKey(nameof(Album))]
    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; } = null!;

    [ForeignKey(nameof(Writer))]
    public int WriterId { get; set; }

    public virtual Writer Writer{ get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    public ICollection<SongPerformer> SongPerformers { get; set; } = new HashSet<SongPerformer>();


}