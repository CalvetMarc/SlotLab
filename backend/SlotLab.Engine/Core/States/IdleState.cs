using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class IdleState : AbstractGameState
    {
        private Action<IGameplayEvent>? _gameplayHandler;

        public IdleState(IGameStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            _gameplayHandler = e => HandleEvent((AbstractEvent)e);
            GameEventBus.Subscribe(_gameplayHandler);
        }

        public override void OnExit()
        {
            if (_gameplayHandler != null)
                GameEventBus.Unsubscribe(_gameplayHandler);

            base.OnExit();
        }

        protected override void HandleEvent(AbstractEvent gameEvent)
        {
            switch (gameEvent)
            {
                case SpinEvent spin:
                    machine.Fire(Trigger.SpinRequested, spin.Bet);
                    break;            
            }
        }
    
    }

}
