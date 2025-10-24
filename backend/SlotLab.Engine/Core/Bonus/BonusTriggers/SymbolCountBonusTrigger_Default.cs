namespace SlotLab.Engine.Core
{
    public class SymbolCountBonusTrigger : IBonusTrigger
    {
        private readonly Dictionary<string, int> symbolsQuantityToTrigger;

        public SymbolCountBonusTrigger(Dictionary<string, int> symbolsQuantityToTrigger)
        {
            this.symbolsQuantityToTrigger = symbolsQuantityToTrigger;
        }

        public (bool, Dictionary<string, object>?) CheckBonusActivation(Dictionary<string, object> gameData)
        {
            if (gameData == null || gameData.Count == 0)
                return (false, null);

            // Inicialitza comptadors per a cada símbol que ens interessa
            var symbolCounters = symbolsQuantityToTrigger.Keys
                .ToDictionary(symbol => symbol, _ => 0);

            // Recorre tots els reels i compta les aparicions
            foreach (var kvp in gameData)
            {
                if (kvp.Value is List<string> symbols)
                {
                    foreach (var symbol in symbols)
                    {
                        foreach (var target in symbolsQuantityToTrigger.Keys)
                        {
                            if (symbol.Equals(target, StringComparison.OrdinalIgnoreCase))
                                symbolCounters[target]++;
                        }
                    }
                }
            }

            // Comprova si s’han complert totes les condicions
            bool triggered = symbolsQuantityToTrigger.All(req =>
                symbolCounters.TryGetValue(req.Key, out int count) && count >= req.Value);

            // Retorna metadades útils si s’ha activat
            return (triggered, triggered ? symbolCounters.ToDictionary(kv => kv.Key, kv => (object)kv.Value) : null);

        }
    }
}
