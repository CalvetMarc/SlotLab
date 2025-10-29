using SlotLab.Engine.Core;
using SlotLab.Engine.Games;

public sealed class MysteriousNightBaseStateFactory : IBaseGameStateFactory
{
    public IdleState CreateIdleState(IGameStateMachine machine, GameEventBus bus, bool IsAuto) => new IdleState(machine, bus, IsAuto);
    public SpinState CreateSpinState(IGameStateMachine machine, GameEventBus bus, Rng rng) => new SpinState(machine, bus, rng);
    public EvaluationState CreateEvaluationState(IGameStateMachine machine, GameEventBus bus) => new EvaluationState_MysteriousNight(machine, bus);
    public PayoutState CreatePayoutState(IGameStateMachine machine, GameEventBus bus, decimal bet) => new PayoutState_MysteriousNight(machine, bus, bet);
}
