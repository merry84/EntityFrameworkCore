using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Diagnose
{
    [Key]
    public int DiagnoseId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(250)]
    public string? Comments { get; set; }

    [Required]
    public int PatientId { get; set; }
    public virtual Patient Patient { get; set; }
}