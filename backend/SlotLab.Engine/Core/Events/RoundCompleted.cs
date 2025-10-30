using SlotLab.Engine.Models;
using SlotLab.Engine.Core;

namespace SlotLab.Engine.Core.Events
{
    public sealed class RoundCompleted : AbstractEvent, IGameplayEvent
    {
        public RoundCompletedMetadata Metadata { get; }

        public RoundCompleted(RoundCompletedMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}