using SlotLab.Engine.Core;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public interface IBaseGameStateFactory
    {
        IdleState CreateIdleState(IGameStateMachine machine, GameEventBus bus, bool IsAuto);
        SpinState CreateSpinState(IGameStateMachine machine, GameEventBus bus, Rng rng);
        EvaluationState CreateEvaluationState(IGameStateMachine machine, GameEventBus bus);
        PayoutState CreatePayoutState(IGameStateMachine machine, GameEventBus bus, decimal bet);
    }
}
