using System;
using System.ComponentModel.DataAnnotations;

namespace MeterLabToolkit.Models;

public class CreatedHistory
{
    public int Id { get; set; }
    
    // Required fields
    [Required]
    public string OpCo { get; set; } = string.Empty;
    
    [Required]
    public string Status { get; set; } = string.Empty;  // 2-digit code from Status table
    
    [Required]
    public string MFR { get; set; } = string.Empty;  // From ManufacturerCode table
    
    [Required]
    public string DevCode { get; set; } = string.Empty;  // From DeviceCode table
    
    // Serial fields - EITHER (BegSer + EndSer) OR (OOR) - never both
    public string? BegSer { get; set; }
    public string? EndSer { get; set; }
    public string? OOR { get; set; }  // Comma-delimited: "10023-10033, 20023-20033" or "123456, 789012"
    
    [Required]
    public int Qty { get; set; }  // Calculated from serials
    
    [Required]
    public DateTime PODate { get; set; }
    
    [Required]
    public string PONumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime RecvDate { get; set; }
    
    [Required]
    public decimal UnitCost { get; set; }
    
    [Required]
    public string CID { get; set; } = string.Empty;
    
    [Required]
    public string MENumber { get; set; } = string.Empty;
    
    [Required]
    public string PurCode { get; set; } = string.Empty;  // From PurchaseCode table
    
    [Required]
    public DateTime Established { get; set; }
    
    public string? Notes { get; set; }  // Optional multi-line notes field
    
    // Navigation/filtering helper
    public string DeviceType { get; set; } = string.Empty;  // "Meter" or "Transformer"
}
