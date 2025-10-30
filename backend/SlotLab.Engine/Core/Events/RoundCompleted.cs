namespace SlotLab.Engine.Core
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