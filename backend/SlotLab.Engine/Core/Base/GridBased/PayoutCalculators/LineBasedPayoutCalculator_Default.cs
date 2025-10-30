using SlotLab.Engine.Models;
using System.Collections.Generic;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Calculates the total payout for line-based evaluation results.
    /// </summary>
    public class LineBasedPayoutCalculator_Default : IGridPayoutCalculator<GridEvaluatorLineBasedOutputRulesData>
    {
        private readonly Data_LineBasedPayoutCalculator lineBasedPayoutCalculatorData;

        /// <summary>
        /// Creates a payout calculator for line-based evaluations.
        /// </summary>
        /// <param name="paytable">Dictionary of symbol → (matchCount → multiplier).</param>
        public LineBasedPayoutCalculator_Default(Data_LineBasedPayoutCalculator lineBasedPayoutCalculatorData)
        {
            this.lineBasedPayoutCalculatorData = lineBasedPayoutCalculatorData;
        }

        /// <summary>
        /// Calculates the total win based on the bet and line evaluation results.
        /// </summary>
        /// <param name="bet">The bet per spin.</param>
        /// <param name="evaluationData">The evaluation result containing all winning lines.</param>
        /// <returns>Total win amount.</returns>
        public decimal Calculate(decimal bet, GridEvaluatorLineBasedOutputRulesData evaluationData)
        {
            decimal totalWin = 0.0m;

            foreach (var (lineNumber, symbol, count) in evaluationData.linesInfo)
            {
                // Validem entrades bàsiques
                if (string.IsNullOrEmpty(symbol) || count < 3)
                    continue;

                // Comprovem que el símbol existeixi a la taula de pagament
                if (lineBasedPayoutCalculatorData.Paytable.TryGetValue(symbol, out var payoutsByCount) &&
                    payoutsByCount.TryGetValue(count, out var multiplier))
                {
                    totalWin += multiplier * bet;
                }
            }

            return totalWin;
        }
    }
}
