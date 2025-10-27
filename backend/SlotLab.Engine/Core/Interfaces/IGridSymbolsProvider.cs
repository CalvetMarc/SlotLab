using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for any slot game's spin logic.
    /// Responsible for generating the visible window and any
    /// additional spin-related data (like stop positions, dynamic reels, etc.).
    /// </summary>
    public interface IGridSymbolsProvider
    {
        /// <summary>
        /// Performs a full play (spin) operation and returns the resulting symbols.
        /// </summary>
        /// <param name="gameStrips">The strips or symbol source for the current game.</param>
        /// <returns>SpinData containing the visible symbols and optional metadata.</returns>
        SpinResultData Spin();
        
    }
    
}
