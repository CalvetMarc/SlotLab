using SlotLab.Engine.Models;
using System.Collections.Generic;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for evaluating a slot game's visible symbols and returning
    /// a typed evaluation result (e.g. lines, clusters, etc.).
    /// </summary>
    /// <typeparam name="TOutput">
    /// The specific type of evaluation result produced by this evaluator.
    /// Must inherit from <see cref="GridEvaluatorOutputRulesData"/>.
    /// </typeparam>
    public interface IGridEvaluator<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        /// <summary>
        /// Evaluates a spin result and returns a typed output describing the win details.
        /// </summary>
        /// <param name="visibleWindow">Matrix of symbols [reel][row]</param>
        /// <param name="bet">Current bet per spin</param>
        /// <returns>Structured evaluation data specific to this mechanic type.</returns>
        TOutput Evaluate(IReadOnlyList<IReadOnlyList<string>> visibleWindow);
    }
}
