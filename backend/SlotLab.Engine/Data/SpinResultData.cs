namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a spin operation — visible symbols + optional metadata.
    /// </summary>
    public class SpinResultData
    {
        /// <summary>
        /// The visible symbols for each column (reel).
        /// [column][row] — e.g., VisibleWindow[reel][symbolIndex]
        /// </summary>
        public List<List<string>> VisibleWindow { get; set; } = new();

        /// <summary>
        /// Optional metadata such as reel stops, layouts, or special spin details.
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; } = new();
    }
}