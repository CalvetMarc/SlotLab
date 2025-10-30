namespace SlotLab.Engine.Core
{
    public sealed class SpinEvent : AbstractEvent, IGameplayEvent
    {
        public SpinRequestedMetadata Metadata { get; }

        public SpinEvent(SpinRequestedMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}