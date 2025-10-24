
namespace SlotLab.Engine.Core
{
    public class ScatterBonusTrigger_Default : IBonusTrigger
    {
        int minScattersToTrigger;
        public ScatterBonusTrigger_Default(int minScattersToTrigger)
        {
            this.minScattersToTrigger = minScattersToTrigger;
        }
        public (bool,Dictionary<string, object>?) CheckBonusActivation(Dictionary<string, object> gameData)
        {
            // Validate input: must contain at least one reel with symbols
            if (gameData == null || gameData.Count == 0)
                return (false, null);

            int scatterCount = 0;

            // Iterate through each reel (key = "Reel1", "Reel2", ...)
            foreach (var kvp in gameData)
            {
                // Ensure the value is a list of symbols (List<string>)
                if (kvp.Value is List<string> symbols)
                {
                    // Count all "Scatter" symbols (no case-insensitive)
                    foreach (var symbol in symbols)
                    {
                        if (symbol.Equals("Scatter", StringComparison.OrdinalIgnoreCase))
                            scatterCount++;
                    }
                }
            }
            bool triggered = scatterCount >= minScattersToTrigger;
            return (triggered, triggered ? new Dictionary<string, object> {{"Scatter", scatterCount }} : null);
        }
    }
}