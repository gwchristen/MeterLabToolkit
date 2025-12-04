using System;

namespace MeterLabToolkit.Models;

public class RmaEntry
{
    public int Id { get; set; }
    public string SerialScan { get; set; } = string.Empty;
    public string MACScan { get; set; } = string.Empty;
    public string Defect { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
}
