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
        protected readonly IGridSymbolsProvider gridSymbolsProvider;
        protected SpinResultData spinResultData = null!;
        public SpinState(IGameStateMachine machine, GameEventBus gameEventBus, Rng rng, IGridSymbolsProvider gridSymbolsProvider) : base(machine, gameEventBus)
        {
            this.rng = rng;
            this.gridSymbolsProvider = gridSymbolsProvider;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            spinResultData = gridSymbolsProvider.Spin();    
            machine.Fire(Trigger.SpinFinished, spinResultData);
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
