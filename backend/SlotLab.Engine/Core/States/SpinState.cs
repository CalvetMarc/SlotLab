using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class SpinState : AbstractGameState
    {
        private readonly decimal _bet;
        public SpinState(IGameStateMachine machine, GameEventBus gameEventBus, decimal bet) : base(machine, gameEventBus)
        {
            _bet = bet;
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
            
        }
    
    }

}
