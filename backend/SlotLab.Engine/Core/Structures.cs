namespace SlotLab.Engine.Core
{
    public enum Trigger
    {
        SpinRequested,
        SpinFinished,
        BonusEntered,
        EvaluationDone
    }

    public record SpinRequestedMetadata(decimal BetAmount, bool IsAuto);
}
