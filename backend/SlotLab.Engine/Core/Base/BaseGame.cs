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
    public abstract class BaseGame : AbstractGameStateMachine
    {
        public readonly GameEventBus gameEventBus = new();
        protected BaseGameData baseGameData { get; init; } = null!;
        protected IBaseGameStateFactory slotStateFactory { get; init; } = null!;

        public BaseGame(ulong? seed = null) { }

        /// <summary>
        /// Define the common state flow and starts the game in the initial Idle state.
        /// </summary>
        public override void InitializeFSM()
        {
            base.InitializeFSM();

            Map<IdleState>(Trigger.SpinRequested, (machine, metadata) => slotStateFactory.CreateSpinState(machine, gameEventBus, baseGameData.NumbersGenerator));
            Map<SpinState>(Trigger.SpinFinished, (machine, metadata) => slotStateFactory.CreateEvaluationState(machine, gameEventBus));
            Map<EvaluationState>(Trigger.EvaluationDone, (machine, metadata) => slotStateFactory.CreatePayoutState(machine, gameEventBus, 2));

            SetInitialState(slotStateFactory.CreateIdleState(this, gameEventBus, baseGameData.IsAuto));
        }                      
    }
}
