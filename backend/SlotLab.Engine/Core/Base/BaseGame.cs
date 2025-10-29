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
        protected readonly Rng rng;
        protected readonly IBaseGameStateFactory slotStateFactory;
        protected bool IsAuto = false;

        protected BaseGame(IBaseGameStateFactory slotStateFactory, ulong? seed = null)
        {
            this.slotStateFactory = slotStateFactory;
            rng = new Rng(seed ?? (ulong)DateTime.UtcNow.Ticks);

            // Define the common state flow
            Map<IdleState>(Trigger.SpinRequested, (machine, metadata) => slotStateFactory.CreateSpinState(machine, gameEventBus, rng));
            Map<SpinState>(Trigger.SpinFinished, (machine, metadata) => slotStateFactory.CreateEvaluationState(machine, gameEventBus));
            Map<EvaluationState>(Trigger.EvaluationDone, (machine, metadata) => slotStateFactory.CreatePayoutState(machine, gameEventBus, 2));
        }

        /// <summary>
        /// Starts the game in the initial Idle state.
        /// </summary>
        public void Start()
        {
            SetInitialState(slotStateFactory.CreateIdleState(this, gameEventBus, IsAuto));
        }        
    }
}
