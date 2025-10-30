using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public interface IGameStateMachine
    {
        void InitializeFSM();
        void Tick();
        void Fire(Trigger trigger, object? metadata = null);        
    }
}