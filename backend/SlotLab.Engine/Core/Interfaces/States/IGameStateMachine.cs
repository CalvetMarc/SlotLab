using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public interface IGameStateMachine
    {
        void InitializeFSM();
        void Tick(GameMechanicInputData input);
        void Fire(Trigger trigger, object? metadata = null);        
    }
}