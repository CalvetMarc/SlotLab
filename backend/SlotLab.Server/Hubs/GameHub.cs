using Microsoft.AspNetCore.SignalR;
using SlotLab.Engine.Core;

namespace SlotLab.Server.Hubs;

public class GameHub : Hub
{
    private readonly GameManager _gameManager;

    public GameHub(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public async Task Spin(double betAmount)
    {
        var result = _gameManager.Spin();
        await Clients.Caller.SendAsync("SpinResult", result);
    }
}
