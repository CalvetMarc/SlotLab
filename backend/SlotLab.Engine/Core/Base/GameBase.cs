using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core.Base
{
    public class GameBase
    {
        protected readonly ISymbolsProvider symbolsProvider;
        protected readonly IGameEvaluator evaluator;

        public GameBase(ISymbolsProvider symbolsProvider, IGameEvaluator evaluator)
        {
            this.symbolsProvider = symbolsProvider;
            this.evaluator = evaluator;
        }

        public virtual PlayResultData Play(double bet = 1.0)
        {
            var spinData = symbolsProvider.Spin();
            double totalWin = evaluator.Evaluate(spinData.VisibleWindow, bet);

            return new PlayResultData
            {
                SpinResultData = spinData,
                TotalWin = totalWin
            };
        }
    }
}
