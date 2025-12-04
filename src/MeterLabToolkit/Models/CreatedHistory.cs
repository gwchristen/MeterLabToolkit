using System;

namespace MeterLabToolkit.Models;

public class CreatedHistory
{
    public int Id { get; set; }
    public string OpCo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string MFR { get; set; } = string.Empty;
    public string DevCode { get; set; } = string.Empty;
    public string BegSer { get; set; } = string.Empty;
    public string EndSer { get; set; } = string.Empty;
    public int Qty { get; set; }
    public DateTime? PODate { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public DateTime? RecvDate { get; set; }
    public decimal? UnitCost { get; set; }
    public string CID { get; set; } = string.Empty;
    public string MENumber { get; set; } = string.Empty;
    public string PurCode { get; set; } = string.Empty;
    public DateTime? Established { get; set; }
    public string Notes { get; set; } = string.Empty;
}
