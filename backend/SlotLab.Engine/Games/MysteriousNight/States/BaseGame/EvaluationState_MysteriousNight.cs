using SlotLab.Engine.Models;
using SlotLab.Engine.Core.Events;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class EvaluationState_MysteriousNight<TOutput> : EvaluationState<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        public EvaluationState_MysteriousNight(IGameStateMachine machine, GameEventBus gameEventBus, List<List<string>> visibleWindow, decimal bet, IGridEvaluator<TOutput> gridEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator)
            : base(machine, gameEventBus, visibleWindow, bet, gridEvaluator, gridPayoutCalculator) { }

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
