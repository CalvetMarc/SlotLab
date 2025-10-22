namespace SlotLab.Server.Core;

/// <summary>
/// Represents a single player's temporary session in demo mode.
/// Stores information about their balance, current game, and connection.
/// </summary>
public class PlayerSession
{
    public string ConnectionId { get; }
    public string PlayerId { get; }
    public string CurrentGameId { get; set; } = "";
    public double Balance { get; set; }
    public double CurrentBet { get; set; } = 1.0; // Default demo bet

    public PlayerSession(string connectionId, string playerId)
    {
        ConnectionId = connectionId;
        PlayerId = playerId;
        Balance = 100_000.00; // Default demo credit
    }

    /// <summary>
    /// Updates the player's bet amount.
    /// </summary>
    public void SetBet(double newBet)
    {
        CurrentBet = newBet;
    }
}
