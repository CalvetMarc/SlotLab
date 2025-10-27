using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Abstract base class implementing common behavior for all game states.
    /// </summary>
    public abstract class AbstractGameState : IGameState
    {     
        public bool IsFinished { get; protected set; }
        public IGameState? NextState { get; protected set; }

        public virtual void HandleEvent(AbstractEvent gameEvent){}

        /// <summary>
        /// Called when the state becomes active.
        /// Use this for initialization or UI setup.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Called right before leaving the state.
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// Executes one logic step in this state.
        /// Must be implemented by each specific state (BaseGame, Bonus, etc.).
        /// </summary>
        public virtual void Tick(GameMechanicInputData input){}

        /// <summary>
        /// Helper for transitioning to another state.
        /// </summary>
        protected void TransitionTo(IGameState nextState)
        {
            IsFinished = true;
            NextState = nextState;
        }
    }
}
