using SlotLab.Engine.Core;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public interface IBonusGameStateFactory
    {
        BonusIntroState CreateIntroState(IGameStateMachine machine, GameEventBus bus, bool IsAuto);
        BonusRoundsState CreateRoundsState(IGameStateMachine machine, GameEventBus bus, Rng rng);
        BonusEndState CreateBonusEndState(IGameStateMachine machine, GameEventBus bus);
    }
}
