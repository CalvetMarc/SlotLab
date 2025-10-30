using SlotLab.Engine.Models;
using SlotLab.Engine.Core;

namespace SlotLab.Engine.Core.Events
{
    public sealed class SpinRequestEvent : AbstractEvent, IGameplayEvent
    {
        public SpinRequestedMetadata Metadata { get; }

        public SpinRequestEvent(SpinRequestedMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}