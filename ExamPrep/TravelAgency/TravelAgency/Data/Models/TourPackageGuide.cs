﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgency.Data.Models;

public class TourPackageGuide
{
    //TourPackageId – integer, Primary Key, foreign key (required)
    // TourPackage – TourPackage
    // GuideId – integer, Primary Key, foreign key (required)
    // Guide – Guide
    [Required]
    [ForeignKey(nameof(TourPackage))]
    public int TourPackageId { get; set; }

    public virtual TourPackage TourPackage { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Guide))]
    public int GuideId { get; set; }
    
    public virtual Guide Guide { get; set; } = null!;
}