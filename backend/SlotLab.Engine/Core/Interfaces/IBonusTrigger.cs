namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for evaluating the current grid (visible symbols)
    /// to determine whether a bonus feature should be triggered.
    /// </summary>
    public interface IBonusTrigger
    {
        /// <summary>
        /// Analyzes the visible window (symbol matrix) of the current spin 
        /// and decides whether the bonus feature should be activated.
        /// </summary>
        /// <param name="visibleWindow">
        /// A matrix representing the visible symbols on the reels, 
        /// </param>
        /// <returns>
        /// <c>true</c> if the current grid state meets the conditions 
        /// required to start the bonus feature; otherwise, <c>false</c>.
        /// </returns>
        public (bool, Dictionary<string, object>?) CheckBonusActivation(Dictionary<string, object> gameData);
    }
}
