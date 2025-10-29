using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class SpinState_MysteriousNight : SpinState
    {
        public SpinState_MysteriousNight(IGameStateMachine machine, GameEventBus gameEventBus, Rng rng) : base(machine, gameEventBus, rng) { }

        public override void OnEnter()
        {
            base.OnEnter();            
        }

        public override void OnExit()
        {           

            base.OnExit();
        }

        protected override void HandleEvent(AbstractEvent gameEvent)
        {
            base.HandleEvent(gameEvent);
        }
    
    }

}
