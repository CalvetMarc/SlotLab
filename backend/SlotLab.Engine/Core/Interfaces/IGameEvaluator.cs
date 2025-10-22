namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Defines the contract for evaluating a slot game's visible symbols and calculating wins.
    /// </summary>
    public interface IGameEvaluator
    {
        /// <summary>
        /// Evaluates a spin result and returns the total win amount.
        /// </summary>
        /// <param name="visibleWindow">Matrix of symbols [reel][row]</param>
        /// <param name="bet">Current bet per spin</param>
        /// <returns>Total win amount for this spin</returns>
        double Evaluate(List<List<string>> visibleWindow, double bet = 1.0);
    }
}
