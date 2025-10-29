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
    public abstract class BonusGame : AbstractGameStateMachine
    {
        public readonly GameEventBus gameEventBus = new();
        protected readonly Rng rng;
        protected readonly IBonusGameStateFactory bonusStateFactory;

        protected BonusGame(IBonusGameStateFactory bonusStateFactory, ulong? seed = null)
        {
            this.bonusStateFactory = bonusStateFactory;
            rng = new Rng(seed ?? (ulong)DateTime.UtcNow.Ticks);

            // Define the common state flow
            Map<BonusIntroState>(Trigger.SpinRequested, (machine, metadata) => bonusStateFactory.CreateRoundsState(machine, gameEventBus, rng));
            Map<BonusRoundsState>(Trigger.SpinFinished, (machine, metadata) => bonusStateFactory.CreateBonusEndState(machine, gameEventBus));
        }

        /// <summary>
        /// Starts the game in the initial Idle state.
        /// </summary>
        public void Start()
        {
            SetInitialState(bonusStateFactory.CreateIntroState(this, gameEventBus, false));
        }        
    }
}
