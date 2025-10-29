using System;
using System.Collections.Generic;
using SlotLab.Engine.Games;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public abstract class AbstractGameStateMachine : IGameStateMachine
    {
        public AbstractGameState CurrentState { get; private set; } = null!;
        private readonly Dictionary<(Type, Trigger), Func<AbstractGameStateMachine, object?, AbstractGameState>> _routes = new(); // (stateType, trigger) => (machine, metadata) -> nextState  
        

        protected void Map<TState>(Trigger trigger, Func<AbstractGameStateMachine, object?, AbstractGameState> factory) where TState : AbstractGameState => _routes[(typeof(TState), trigger)] = factory; //Paths registry

        public void Fire(Trigger trigger, object? metadata = null)
        {
            var key = (CurrentState.GetType(), trigger);
            if (_routes.TryGetValue(key, out var factory))
            {
                var next = factory(this, metadata);
                ChangeState(next);
            }
            else
            {
                // Opcional: log o ignorar
                // Console.WriteLine($"No route for ({CurrentState.GetType().Name}, {trigger})");
            }
        }

        public void SetInitialState(AbstractGameState newState)
        {
            CurrentState = newState ?? throw new ArgumentNullException(nameof(newState));
            CurrentState.OnEnter();
        }

        public void Tick(GameMechanicInputData input)
        {
            if (CurrentState == null)
                throw new InvalidOperationException("State machine has no active state.");

            CurrentState.Tick(input);
        }

        public void ChangeState(AbstractGameState newState)
        {
            if (newState == null) throw new ArgumentNullException(nameof(newState));
            
            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState.OnEnter();
        }
    }
}
