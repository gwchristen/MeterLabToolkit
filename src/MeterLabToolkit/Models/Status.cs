namespace MeterLabToolkit.Models;

public class Status
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;  // 2-digit code
    public string Description { get; set; } = string.Empty;
}
