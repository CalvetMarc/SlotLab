using SlotLab.Engine.Core;
using SlotLab.Engine.Games;
using SlotLab.Engine.Models;

public sealed class MysteriousNightBaseStateFactory<TOutput> : IBaseGameStateFactory<TOutput> where TOutput : GridEvaluatorOutputRulesData
{
    public IdleState CreateIdleState(IGameStateMachine machine, GameEventBus bus, bool IsAuto, GameEnvironmentMode gameEnvironmentMode) => new IdleState(machine, bus, IsAuto, gameEnvironmentMode);
    public SpinState CreateSpinState(IGameStateMachine machine, GameEventBus bus, Rng rng, IGridSymbolsProvider gridSymbolsProvider) => new SpinState(machine, bus, rng, gridSymbolsProvider);
    public EvaluationState<TOutput> CreateEvaluationState(IGameStateMachine machine, GameEventBus bus, decimal bet, List<List<string>> visibleWindow, IGridEvaluator<TOutput> gridEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator) =>
        new EvaluationState_MysteriousNight<TOutput>(machine, bus, visibleWindow, bet, gridEvaluator, gridPayoutCalculator);
    public PayoutState<TOutput> CreatePayoutState(IGameStateMachine machine, GameEventBus bus, decimal payoutAmount, GameEnvironmentMode gameEnvironmentMode) =>
        new PayoutState_MysteriousNight<TOutput>(machine, bus, payoutAmount, gameEnvironmentMode);
}
