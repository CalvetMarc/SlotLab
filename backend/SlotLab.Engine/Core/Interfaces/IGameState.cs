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
        /// Executes one logical "tick" or frame of the game flow.
        /// Typically, this represents one spin or action within this state.
        /// </summary>
        void Tick(GameMechanicInputData input);

        /// <summary>
        /// Handles external events coming from other layers (UI, API, system, etc.).
        /// This allows the game flow to be event-driven and not directly tied to a single input source.
        /// </summary>
        /// <param name="gameEvent">The event data received from the outside world.</param>
        void HandleEvent(AbstractEvent gameEvent);

        /// <summary>
        /// Whether this state has completed its logic and should transition to another state.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Reference to the next state to transition to (if any).
        /// </summary>
        IGameState? NextState { get; }

        /// <summary>
        /// Called when entering this state (initialization or setup).
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called when leaving this state (cleanup or transition preparation).
        /// </summary>
        void OnExit();
    }
}
