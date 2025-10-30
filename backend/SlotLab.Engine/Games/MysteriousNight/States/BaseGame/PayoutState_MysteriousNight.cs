using SlotLab.Engine.Models;
using SlotLab.Engine.Core.Events;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class PayoutState_MysteriousNight<TOutput> : PayoutState<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        public PayoutState_MysteriousNight(IGameStateMachine machine, GameEventBus gameEventBus, decimal payoutAmmount, GameEnvironmentMode gameEnvironmentMode) : base(machine, gameEventBus, payoutAmmount, gameEnvironmentMode) { }

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
