using SlotLab.Engine.Models;
using SlotLab.Engine.Core.Events;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class IdleState : AbstractGameState
    {
        protected Action<SpinRequestEvent>? _gameplayHandler;
        protected readonly bool IsAuto;
        protected readonly GameEnvironmentMode gameEnvironmentMode;

        public IdleState(IGameStateMachine machine, GameEventBus gameEventBus, bool IsAuto, GameEnvironmentMode gameEnvironmentMode) : base(machine, gameEventBus)
        {
            this.IsAuto = IsAuto;
            this.gameEnvironmentMode = gameEnvironmentMode;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (gameEnvironmentMode == GameEnvironmentMode.Simulation)
            {
                var metadata = new SpinRequestedMetadata(BetAmount: 1.0m, IsAuto: true);
                machine.Fire(Trigger.SpinRequested, metadata);
                return;
            }

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
                case SpinRequestEvent spin:
                    machine.Fire(Trigger.SpinRequested, spin.Metadata);
                    break;
            }
        }

    }

}
