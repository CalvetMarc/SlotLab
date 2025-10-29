using SlotLab.Engine.Core;
using SlotLab.Engine.Games;

public sealed class MysteriousNightBonusStateFactory : IBonusGameStateFactory
{
    public BonusIntroState CreateIntroState(IGameStateMachine machine, GameEventBus bus, bool IsAuto) => new BonusIntroState_MysteriousNight(machine, bus);
    public BonusRoundsState CreateRoundsState(IGameStateMachine machine, GameEventBus bus, Rng rng) => new BonusRoundsState_MysteriousNight(machine, bus);
    public BonusEndState CreateBonusEndState(IGameStateMachine machine, GameEventBus bus) => new BonusEndState_MysteriousNight(machine, bus);
}
