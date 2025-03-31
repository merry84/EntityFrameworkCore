﻿using System.ComponentModel.DataAnnotations;
using NetPay.Data.Models;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos;

[XmlType(nameof(Expense))]
public class ExportExpenseDto
{
   
    [XmlElement("ExpenseName")]
    [StringLength(50, MinimumLength = 5)]
    public string ExpenseName { get; set; }= null!;

    [XmlElement("Amount")]
    [Range(0.01, 100000)]
    public string Amount { get; set; }= null!;

    [XmlElement("PaymentDate")]
    public string PaymentDate { get; set; }= null!;

    [XmlElement("ServiceName")]
    [StringLength(30, MinimumLength = 5)]
    public string ServiceName { get; set; } = null!;

}