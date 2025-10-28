namespace SlotLab.Engine.Core
{
    public sealed class BonusTriggeredEvent : AbstractEvent, IBonusEvent
    {
        public string BonusId { get; init; } = "";
    }
}