namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public class PlayResultData
    {        
        public SpinResultData SpinResultData { get; set; } = new();
        
        public double TotalWin { get; set; } = new();
    }
}