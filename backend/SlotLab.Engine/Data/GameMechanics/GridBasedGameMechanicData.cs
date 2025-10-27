using SlotLab.Engine.Models;

namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Represents the full result of a grid-based game mechanic execution — 
    /// including evaluation result and total win amount.
    /// </summary>
    /// <typeparam name="TOutput">
    /// The type of evaluation result (e.g. lines, clusters, etc.).
    /// </typeparam>
    public class GridBasedGameMechanicOutputData<TOutput> : GameMechanicOutputData
        where TOutput : GridEvaluatorOutputRulesData
    {
        /// <summary>
        /// The evaluation result produced by the evaluator.
        /// </summary>
        public TOutput EvaluationResult { get; set; }

        /// <summary>
        /// The total amount won in this spin.
        /// </summary>
        public double WinAmount { get; set; }

        public GridBasedGameMechanicOutputData(double WinAmount, TOutput EvaluationResult){
            this.WinAmount = WinAmount;
            this.EvaluationResult = EvaluationResult;
        }
    }
}
