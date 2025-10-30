namespace SlotLab.Engine.Core
{
    public enum Trigger
    {
        SpinRequested,
        SpinFinished,
        BonusEntered,
        EvaluationDone,
        TransactionDone,
    }

    public record SpinRequestedMetadata(decimal BetAmount, bool IsAuto);
    public record RoundCompletedMetadata(decimal PayoutAmount, decimal PayoutFromBase, decimal PayoutFromBonus);
}
