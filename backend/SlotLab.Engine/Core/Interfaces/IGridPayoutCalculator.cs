using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for calculating the payout of a grid evaluation result.
    /// </summary>
    /// <typeparam name="TOutput">
    /// The specific type of evaluation result that this payout calculator can handle.
    /// Must inherit from <see cref="GridEvaluatorOutputData"/>.
    /// </typeparam>
    public interface IGridPayoutCalculator<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        /// <summary>
        /// Calculates the payout based on the bet amount and the evaluation result.
        /// </summary>
        /// <param name="bet">Current bet per spin.</param>
        /// <param name="evaluationData">The evaluation output containing win details.</param>
        /// <returns>The total win amount for this spin.</returns>
        double Calculate(double bet, TOutput evaluationData);
    }
}
