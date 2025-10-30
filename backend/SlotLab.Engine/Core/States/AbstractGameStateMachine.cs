using System;
using System.Collections.Generic;
using SlotLab.Engine.Games;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public abstract class AbstractGameStateMachine : IGameStateMachine
    {
        protected AbstractGameState CurrentState { get; private set; } = null!;
        private readonly Dictionary<(Type, Trigger), Func<AbstractGameStateMachine, object?, AbstractGameState>> _routes = new(); // (stateType, trigger) => (machine, metadata) -> nextState  
        

        protected void Map<TState>(Trigger trigger, Func<AbstractGameStateMachine, object?, AbstractGameState> factory) where TState : AbstractGameState => _routes[(typeof(TState), trigger)] = factory; //Paths registry


        public virtual void InitializeFSM() {}

        public void Tick()
        {
            if (CurrentState == null)
                throw new InvalidOperationException("State machine has no active state.");

            CurrentState.Tick();
        }

        public void Fire(Trigger trigger, object? metadata = null)
        {
            var stateType = CurrentState.GetType();
            var key = (stateType, trigger);

            // 1️⃣ Primer intent: coincidència exacta
            if (_routes.TryGetValue(key, out var factory))
            {
                var next = factory(this, metadata);
                ChangeState(next);
                return;
            }

            // 2️⃣ Segon intent: si és genèric, prova amb el tipus obert (p. ex. EvaluationState<>)
            if (stateType.IsGenericType)
            {
                var openGeneric = stateType.GetGenericTypeDefinition();
                var openKey = (openGeneric, trigger);
                if (_routes.TryGetValue(openKey, out factory))
                {
                    var next = factory(this, metadata);
                    ChangeState(next);
                    return;
                }
            }

            // 3️⃣ Tercer intent: buscar a les classes base (per subclasses heretades)
            var baseType = stateType.BaseType;
            while (baseType != null)
            {
                var baseKey = (baseType, trigger);
                if (_routes.TryGetValue(baseKey, out factory))
                {
                    var next = factory(this, metadata);
                    ChangeState(next);
                    return;
                }

                if (baseType.IsGenericType)
                {
                    var openBase = baseType.GetGenericTypeDefinition();
                    var openBaseKey = (openBase, trigger);
                    if (_routes.TryGetValue(openBaseKey, out factory))
                    {
                        var next = factory(this, metadata);
                        ChangeState(next);
                        return;
                    }
                }

                baseType = baseType.BaseType;
            }

            // 4️⃣ Sense coincidència — log informatiu
            Console.WriteLine($"⚠️ No route for ({stateType.Name}, {trigger})");
        }


        protected virtual void SetInitialState(AbstractGameState newState)
        {
            CurrentState = newState ?? throw new ArgumentNullException(nameof(newState));
            CurrentState.OnEnter();
        }        
        
        protected virtual void ChangeState(AbstractGameState newState)
        {
            if (newState == null) throw new ArgumentNullException(nameof(newState));
            
            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState.OnEnter();
        }        
        
    }
}
