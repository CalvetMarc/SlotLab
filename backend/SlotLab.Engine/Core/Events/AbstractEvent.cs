using SlotLab.Engine.Models;
using SlotLab.Engine.Core;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Base class for all game events.
    /// Can carry common metadata useful for logging or debugging.
    /// </summary>
    public abstract class AbstractEvent
    {
        /// <summary>
        /// When the event was created (UTC time).
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Optional identifier or source component name that produced this event.
        /// </summary>
        public string? Source { get; init; }
    }
}
