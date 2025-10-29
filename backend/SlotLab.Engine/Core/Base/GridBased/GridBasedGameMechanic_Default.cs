using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core.Base
{
    /// <summary>
    /// Base implementation of a grid-based game mechanic that spins symbols,
    /// evaluates the result, and calculates the payout.
    /// </summary>
    /// <typeparam name="TOutput">
    /// The specific evaluation output type produced by the evaluator and used by the payout calculator.
    /// </typeparam>
    public class GridBasedGameMechanic_Default<TOutput>  where TOutput : GridEvaluatorOutputRulesData
    {
        public IGridSymbolsProvider SymbolsProvider { get; }
        public IGridEvaluator<TOutput> GameEvaluator { get; }
        public IGridPayoutCalculator<TOutput> GridPayoutCalculator { get; }

        public GridBasedGameMechanic_Default(IGridSymbolsProvider symbolsProvider, IGridEvaluator<TOutput> gameEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator)
        {
            this.SymbolsProvider = symbolsProvider;
            this.GameEvaluator = gameEvaluator;
            this.GridPayoutCalculator = gridPayoutCalculator;
        }        
    }
}
