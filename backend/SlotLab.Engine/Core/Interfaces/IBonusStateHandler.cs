namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for handling the lifecycle of a bonus feature
    /// within the slot engine. A bonus typically consists of three phases:
    /// entering, updating its state (spins, respins, picks, etc.), and exiting
    /// while returning the total accumulated win value.
    /// </summary>
    public interface IBonusStateHandler
    {
        /// <summary>
        /// Called when the bonus feature starts. Initializes its internal state
        /// (for example, number of free spins, respins, picks, etc.)
        /// based on metadata provided by the trigger that activated it.
        /// </summary>
        /// <param name="metadata">
        /// Optional bonus initialization data returned by the trigger —
        /// such as number of scatters, start spins, or multipliers.
        /// </param>
        void Enter(Dictionary<string, object>? metadata);

        /// <summary>
        /// Executes one "interaction" of the bonus feature — for example,
        /// one free spin, respin, pick, or progression step — updating its state
        /// and returning both its current win and whether the bonus continues.
        /// </summary>
        /// <param name="bet">
        /// The base bet value used for all internal win calculations.
        /// </param>
        /// <returns>
        /// A tuple where:
        /// <list type="bullet">
        ///   <item><c>active</c> — indicates whether the bonus should continue (true) or has ended (false).</item>
        ///   <item><c>win</c> — represents the win obtained during this interaction.</item>
        /// </list>
        /// </returns>
        void Update(Dictionary<string, object>? metadata);

        /// <summary>
        /// Called once when the bonus feature finishes execution.
        /// Used to finalize and return the total accumulated win value
        /// over all bonus interactions.
        /// </summary>
        /// <returns>
        /// The total win amount obtained during the entire bonus round,
        /// expressed in the same currency units as the current bet.
        /// </returns>
        bool HasBonusFinished();
    }
}
