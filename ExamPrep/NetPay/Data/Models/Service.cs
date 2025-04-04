﻿using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models;

public class Service
{
    //Id – integer, Primary Key
    // ServiceName – text with length [5, 30] (required)
    // Expenses - a collection of type Expense
    // SuppliersServices - collection of type SupplierService

    [Key]
    public int Id  { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 5)]
    public string ServiceName { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; }
        = new HashSet<Expense>();
    public virtual ICollection<SupplierService> SuppliersServices { get; set; } 
        = new HashSet<SupplierService>();

}