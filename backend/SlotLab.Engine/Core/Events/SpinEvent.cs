namespace SlotLab.Engine.Core
{
    public sealed class SpinEvent : AbstractEvent, IGameplayEvent
    {
        public double Bet { get; init; }
    }
}