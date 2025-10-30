using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class EvaluationState<TOutput> : AbstractGameState where TOutput : GridEvaluatorOutputRulesData
    {
        protected readonly decimal bet;
        protected IReadOnlyList<IReadOnlyList<string>> visibleWindow;
        protected readonly IGridPayoutCalculator<TOutput> gridPayoutCalculator;
        protected readonly IGridEvaluator<TOutput> gridEvaluator;

        public EvaluationState(IGameStateMachine machine, GameEventBus gameEventBus, List<List<string>> visibleWindow, decimal bet, IGridEvaluator<TOutput> gridEvaluator, IGridPayoutCalculator<TOutput> gridPayoutCalculator) : base(machine, gameEventBus)
        {
            this.gridEvaluator = gridEvaluator;
            this.visibleWindow = visibleWindow;
            this.bet = bet;
            this.gridPayoutCalculator = gridPayoutCalculator;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            TOutput evaluationData = gridEvaluator.Evaluate(visibleWindow);
            decimal payoutAmmount = gridPayoutCalculator.Calculate(bet, evaluationData);
            machine.Fire(Trigger.SpinFinished, payoutAmmount);
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
