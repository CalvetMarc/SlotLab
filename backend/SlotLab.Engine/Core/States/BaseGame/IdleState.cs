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
        private readonly bool IsAuto;

        public IdleState(IGameStateMachine machine, GameEventBus gameEventBus, bool IsAuto) : base(machine, gameEventBus)
        {
            this.IsAuto = IsAuto;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _gameplayHandler = e => HandleEvent((AbstractEvent)e);
            gameEventBus.Subscribe(_gameplayHandler);
        }

        public override void OnExit()
        {
            if (_gameplayHandler != null)
                gameEventBus.Unsubscribe(_gameplayHandler);

            base.OnExit();
        }

        protected override void HandleEvent(AbstractEvent gameEvent)
        {
            switch (gameEvent)
            {
                case SpinEvent spin:
                    machine.Fire(Trigger.SpinRequested, spin.Metadata);
                    break;            
            }
        }
    
    }

}
