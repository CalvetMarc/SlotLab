namespace SlotLab.Engine.Core
{
    public sealed class BonusTriggeredEvent : GameEvent, IBonusEvent
    {
        public string BonusId { get; init; } = "";
    }
}