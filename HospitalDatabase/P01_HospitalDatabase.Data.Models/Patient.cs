using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace P01_HospitalDatabase.Data.Models;

public class Patient
{
    //PatientId
    // FirstName (up to 50 characters, unicode)
    // LastName (up to 50 characters, unicode)
    // Address (up to 250 characters, unicode)
    // Email (up to 80 characters, not unicode)
    // HasInsurance

    [Key]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(50)]
    [Unicode]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Unicode]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(250)]
    [Unicode]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    [Unicode(false)]
    public string Email { get; set; } = null!;


    public bool HasInsurance { get; set; }

    public virtual ICollection<Diagnose> Diagnoses { get; set; } = new HashSet<Diagnose>();
    public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();

    public virtual ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();


}