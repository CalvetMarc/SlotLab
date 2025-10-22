namespace SlotLab.Engine.Models
{
    public class SpinResult
    {
        /// <summary>
        /// Reels before the visible result (used to animate the entry of symbols).
        /// </summary>
        public IEnumerable<object> PreRoll { get; set; } = new List<object>();

        /// <summary>
        /// Visible window of the game (e.g., the 3x5 symbols shown in the center).
        /// </summary>
        public IEnumerable<object> VisibleWindow { get; set; } = new List<object>();

        /// <summary>
        /// Reels after the visible result (used to animate the exit of symbols).
        /// </summary>
        public IEnumerable<object> PostRoll { get; set; } = new List<object>();

        /// <summary>
        /// Total amount won in this spin.
        /// </summary>
        public double WinAmount { get; set; }
    }
}
