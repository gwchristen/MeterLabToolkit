using System;

namespace MeterLabToolkit.Models;

public class CellularRegistration
{
    public int Id { get; set; }
    public string Device { get; set; } = string.Empty;
    public string MAC { get; set; } = string.Empty;
    public string Serial { get; set; } = string.Empty;
    public string IP { get; set; } = string.Empty;
    public string IMEI { get; set; } = string.Empty;
    public string SimIccid { get; set; } = string.Empty;
    public string MDN { get; set; } = string.Empty;
    public string MIN { get; set; } = string.Empty;
    public string APN { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
}
