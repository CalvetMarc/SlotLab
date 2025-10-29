using SlotLab.Engine.Core;
using SlotLab.Engine.Games;

public sealed class MysteriousNightStateFactory : IBaseGameStateFactory
{
    public IdleState CreateIdleState(IGameStateMachine machine, GameEventBus bus, bool IsAuto) => new IdleState(machine, bus, IsAuto);
    public SpinState CreateSpinState(IGameStateMachine machine, GameEventBus bus, Rng rng) => new SpinState(machine, bus, rng);
    public EvaluationState CreateEvaluationState(IGameStateMachine machine, GameEventBus bus)
    {
        throw new NotImplementedException();
    }

    public PayoutState CreatePayoutState(IGameStateMachine machine, GameEventBus bus, decimal bet)
    {
        throw new NotImplementedException();
    }
}
