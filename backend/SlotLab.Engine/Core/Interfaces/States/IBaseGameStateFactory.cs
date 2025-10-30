using SlotLab.Engine.Core;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public interface IBaseGameStateFactory<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        IdleState CreateIdleState(IGameStateMachine machine, GameEventBus bus, bool IsAuto, GameEnvironmentMode gameEnvironmentMode);
        SpinState CreateSpinState(IGameStateMachine machine, GameEventBus bus, Rng rng, IGridSymbolsProvider gridSymbolsProvider);
        EvaluationState<TOutput> CreateEvaluationState(IGameStateMachine machine, GameEventBus bus, decimal bet,
            List<List<string>> visibleWindow, IGridEvaluator<TOutput> gridEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator);
        PayoutState<TOutput> CreatePayoutState(IGameStateMachine machine, GameEventBus bus, decimal payoutAmount, GameEnvironmentMode gameEnvironmentMode);
    }
}
