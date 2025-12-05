namespace MeterLabToolkit.Models;

public class ManufacturerCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;  // Internal 2-digit code
    public string IndustryCode { get; set; } = string.Empty;  // Industry 1-digit code
    public string Name { get; set; } = string.Empty;  // Full manufacturer name
}
