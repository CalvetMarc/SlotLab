namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Represents player-related data used during gameplay,
    /// such as available credits and the random seed for reproducible spins.
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// The random number generator seed used to produce deterministic spin outcomes.
        /// </summary>
        public int rngSeed;

        /// <summary>
        /// The current amount of credits available to the player.
        /// </summary>
        public int credit;
    }
}
