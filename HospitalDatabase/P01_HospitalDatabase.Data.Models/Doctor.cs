using System.ComponentModel.DataAnnotations;
using P01_HospitalDatabase.Data.Models;

public class Doctor
{
    

    [Key]
    public int DoctorId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Specialty { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }  // Ще съхраняваме хеширана парола

    public virtual ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();
}