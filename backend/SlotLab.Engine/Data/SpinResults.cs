namespace SlotLab.Engine.Models;

public class SpinResult
{
    public string Game { get; set; } = "";
    public int Rows { get; set; }
    public int Columns { get; set; }
    public IEnumerable<object> Reels { get; set; } = new List<object>();
    public double WinAmount { get; set; }
    public DateTime Timestamp { get; set; }
}
