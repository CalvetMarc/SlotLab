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
    public class GridBasedGameMechanic_Default<TOutput> : IGameMechanic  where TOutput : GridEvaluatorOutputRulesData
    {
        protected readonly IGridSymbolsProvider symbolsProvider;
        protected readonly IGridEvaluator<TOutput> gameEvaluator;
        protected readonly IGridPayoutCalculator<TOutput> gridPayoutCalculator;

        public GridBasedGameMechanic_Default(IGridSymbolsProvider symbolsProvider, IGridEvaluator<TOutput> gameEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator)
        {
            this.symbolsProvider = symbolsProvider;
            this.gameEvaluator = gameEvaluator;
            this.gridPayoutCalculator = gridPayoutCalculator;
        }

        public GameMechanicOutputData Tick(GameMechanicInputData gameMechanicInputData)
        {
            // 1. Get new symbols from the spin
            SpinResultData spinData = symbolsProvider.Spin();

            // 2. Evaluate the visible symbols using the correct evaluator
            TOutput evaluationResult = gameEvaluator.Evaluate(spinData.VisibleWindow);

            // 3. Calculate payout using the matching payout calculator
            double win = gridPayoutCalculator.Calculate(gameMechanicInputData.bet, evaluationResult);

            // 4. Package the result
            return new GridBasedGameMechanicOutputData<TOutput>(win, evaluationResult);
        }
    }
}
