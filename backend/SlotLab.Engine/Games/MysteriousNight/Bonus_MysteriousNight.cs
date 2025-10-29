using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using SlotLab.Engine.Models;
using SlotLab.Engine.Games;


/// <summary>
/// Game state machine for the "MysteriousNight" slot.
/// Declares the flow with a route table: (State, Trigger) -> Next State factory.
/// </summary>
public sealed class Bonus_MysteriousNight : BonusGame
{       
    public Bonus_MysteriousNight(ulong? seed = null) : base(new MysteriousNightBonusStateFactory(), seed)
    {
        
    }
}