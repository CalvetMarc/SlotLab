using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core.Base
{
    public abstract class GameBase
    {
        protected readonly ISymbolsProvider symbolsProvider;
        protected readonly IGameEvaluator evaluator;

        protected GameBase(ISymbolsProvider symbolsProvider, IGameEvaluator evaluator)
        {
            this.symbolsProvider = symbolsProvider;
            this.evaluator = evaluator;
        }

        public virtual PlayResultData Play(List<List<string>> strips, double bet = 1.0)
        {
            var spinData = symbolsProvider.Spin(strips);
            double totalWin = evaluator.Evaluate(spinData.VisibleWindow, bet);

            return new PlayResultData
            {
                SpinResultData = spinData,
                TotalWin = totalWin
            };
        }
    }
}
