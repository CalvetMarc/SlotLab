using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    /// <summary>
    /// Base class for all slot games.
    /// Defines the common flow (Idle → Spin → Evaluate → Payout).
    /// Derived games can override the state creation methods to customize behavior.
    /// </summary>
    public abstract class BaseGame<TOutput> : AbstractGameStateMachine where TOutput : GridEvaluatorOutputRulesData
    {
        public readonly GameEventBus gameEventBus = new();
        protected BaseGameData<TOutput> baseGameData { get; init; } = null!;
        protected IBaseGameStateFactory<TOutput> slotStateFactory { get; init; } = null!;

        public BaseGame(GridBasedGameMechanic_Default<TOutput> mechanic, GameEnvironmentMode gameEnvironmentMode, ulong? seed = null)
        {
            baseGameData = new BaseGameData<TOutput>(mechanic, new Rng(seed ?? (ulong)DateTime.UtcNow.Ticks), gameEnvironmentMode);
        }

        /// <summary>
        /// Define the common state flow and starts the game in the initial Idle state.
        /// </summary>
        public override void InitializeFSM()
        {
            base.InitializeFSM();

            Map<IdleState>(Trigger.SpinRequested, (machine, metadata) => slotStateFactory.CreateSpinState(machine, gameEventBus, baseGameData.NumbersGenerator, baseGameData.GridGameMechanicComponents.SymbolsProvider));
            Map<SpinState>(Trigger.SpinFinished, (machine, metadata) => slotStateFactory.CreateEvaluationState(
                machine, gameEventBus, baseGameData.ActiveBet, ((SpinResultData)metadata!).VisibleWindow, baseGameData.GridGameMechanicComponents.GameEvaluator, baseGameData.GridGameMechanicComponents.GridPayoutCalculator
            ));
            Map<EvaluationState<TOutput>>(Trigger.EvaluationDone, (machine, metadata) => slotStateFactory.CreatePayoutState(machine, gameEventBus, (decimal)metadata!, baseGameData.GameEnvMode));
            Map<PayoutState<TOutput>>(Trigger.TransactionDone, (machine, metadata) => slotStateFactory.CreateIdleState(machine, gameEventBus, baseGameData.IsAuto, baseGameData.GameEnvMode));

            SetInitialState(slotStateFactory.CreateIdleState(this, gameEventBus, baseGameData.IsAuto, baseGameData.GameEnvMode));
        }                      
    }
}
