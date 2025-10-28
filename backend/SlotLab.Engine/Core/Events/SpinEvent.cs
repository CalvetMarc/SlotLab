namespace SlotLab.Engine.Core
{
    public sealed class SpinEvent : AbstractEvent, IGameplayEvent
    {
        public decimal Bet { get; init; }
    }
}