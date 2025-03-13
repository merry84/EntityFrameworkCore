﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace P01_HospitalDatabase.Data.Models;

public class Medicament
{
    //MedicamentId
    // Name (up to 50 characters, unicode)
    [Key]
    public int MedicamentId { get; set; }

    [Required] 
    [MaxLength(50)]
    [Unicode]
    public string Name { get; set; } = null!;

    public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
}