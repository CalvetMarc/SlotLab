using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Base interface for all game states.
    /// Each state represents a specific phase of the slot game flow.
    /// States are driven by both game ticks (internal logic) and external events.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Called when entering this state (initialization or setup).
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Executes one logical "tick" or frame of the game flow.
        /// Typically, this represents one spin or action within this state.
        /// </summary>
        void Tick();  
        
        /// <summary>
        /// Called when leaving this state (cleanup or transition preparation).
        /// </summary>
        void OnExit();
    }
}
