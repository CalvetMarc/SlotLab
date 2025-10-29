using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class BonusIntro : AbstractGameState
    {

        public BonusIntro(IGameStateMachine machine, GameEventBus gameEventBus) : base(machine, gameEventBus)
        {
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
