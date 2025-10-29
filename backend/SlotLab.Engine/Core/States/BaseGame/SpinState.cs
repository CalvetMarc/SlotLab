using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class SpinState : AbstractGameState
    {
        protected readonly Rng rng;
        public SpinState(IGameStateMachine machine, GameEventBus gameEventBus, Rng rng) : base(machine, gameEventBus)
        {
            this.rng = rng;
        }

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
