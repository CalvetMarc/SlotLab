using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for any slot game's spin logic.
    /// Responsible for generating the visible window and any
    /// additional spin-related data (like stop positions, dynamic reels, etc.).
    /// </summary>
    public interface ISymbolsProvider
    {
        /// <summary>
        /// Performs a full play (spin) operation and returns the resulting symbols.
        /// </summary>
        /// <param name="gameStrips">The strips or symbol source for the current game.</param>
        /// <returns>SpinData containing the visible symbols and optional metadata.</returns>
        SpinResultData Spin(List<List<string>> gameStrips);
        /// <summary>
        /// Safely gets a symbol from a reel using circular (wrap-around) indexing.
        /// </summary>
        /// <param name="reel">The reel (strip) to read from.</param>
        /// <param name="index">The target index, can be negative or exceed reel length.</param>
        /// <returns>The symbol at the wrapped position.</returns>
        string GetSymbolCircular(List<string> reel, int index)
        {
            if (reel == null || reel.Count == 0)
                throw new ArgumentException("Reel cannot be null or empty", nameof(reel));

            int normalizedIndex = ((index % reel.Count) + reel.Count) % reel.Count;
            return reel[normalizedIndex];
        }
    }
    
}
