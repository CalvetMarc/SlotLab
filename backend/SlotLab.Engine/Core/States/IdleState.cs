using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Represents the idle state between spins. 
    /// It waits for external events (e.g. PlayerSpin, AutoSpin) to trigger the next state.
    /// </summary> 
    public class IdleState : AbstractGameState
{
    private Action<IGameplayEvent>? _gameplayHandler;

    public override void OnEnter()
    {
        base.OnEnter();

        _gameplayHandler = e => HandleEvent(e);
        GameEventBus.Subscribe(_gameplayHandler);
    }

    public override void OnExit()
    {
        if (_gameplayHandler != null)
            GameEventBus.Unsubscribe(_gameplayHandler);

        base.OnExit();
    }

    public override void HandleEvent(AbstractEvent gameEvent)
    {
        switch (gameEvent)
        {
            case PlayerSpinEvent spin:
                TransitionTo(new SpinState(spin.Bet));
                break;

            case AutoSpinEvent autoSpin:
                TransitionTo(new SpinState(autoSpin.Bet, isAutoSpin: true));
                break;
        }
    }
}

}
