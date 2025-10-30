using SlotLab.Engine.Models;
using SlotLab.Engine.Core.Events;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class PayoutState<TOutput> : AbstractGameState where TOutput : GridEvaluatorOutputRulesData
    {
        protected Action<AnimatingFinished>? _gameplayHandler;
        protected readonly decimal payoutAmount;
        protected readonly GameEnvironmentMode gameEnvironmentMode;

        public PayoutState(IGameStateMachine machine, GameEventBus gameEventBus, decimal payoutAmount, GameEnvironmentMode gameEnvironmentMode) : base(machine, gameEventBus)
        {
            this.payoutAmount = payoutAmount;
            this.gameEnvironmentMode = gameEnvironmentMode;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (gameEnvironmentMode == GameEnvironmentMode.Simulation)
            {
                machine.Fire(Trigger.TransactionDone, null);
                return;
            }

            _gameplayHandler = e => HandleEvent((AbstractEvent)e);
            gameEventBus.Subscribe(_gameplayHandler);            
        }

        public override void OnExit()
        {
            if (_gameplayHandler != null)
                gameEventBus.Unsubscribe(_gameplayHandler);

            gameEventBus.Publish(new RoundCompleted(new RoundCompletedMetadata(payoutAmount, payoutAmount, 0)));
            base.OnExit();            
        }

        protected override void HandleEvent(AbstractEvent gameEvent)
        {
            switch (gameEvent)
            {
                case AnimatingFinished animatingFinished:
                    machine.Fire(Trigger.TransactionDone, new RoundCompletedMetadata(payoutAmount, payoutAmount, 0));
                    break;            
            }
        }
    
    }

}
