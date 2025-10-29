using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public interface IGameStateMachine
    {
        AbstractGameState CurrentState { get; }

        void SetInitialState(AbstractGameState newState);
        void Tick(GameMechanicInputData input);
        void ChangeState(AbstractGameState newState);
        void Fire(Trigger trigger, object? metadata = null);
    }
}