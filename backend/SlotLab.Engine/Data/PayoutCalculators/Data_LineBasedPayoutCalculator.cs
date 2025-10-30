using System.Text.Json.Nodes;

namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Config DTO for GridReelsSymbolsProvider (immutable).
    /// </summary>
    public sealed class Data_LineBasedPayoutCalculator
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<int, Decimal>> Paytable { get; init; } = new Dictionary<string, IReadOnlyDictionary<int, decimal>>();       

        private Data_LineBasedPayoutCalculator() { }

        public static Data_LineBasedPayoutCalculator Load(JsonNode root)
        {
            if (root is null)
                throw new InvalidOperationException("JSON root is null.");           

            // --- PAYTABLE ---
            var paytable = new Dictionary<string, Dictionary<int, decimal>>();
            var paytableNode = root["paytable"]?.AsArray();
            if (paytableNode != null)
            {
                foreach (var symbol in paytableNode)
                {
                    string symbolName = symbol!["Symbol"]!.GetValue<string>();
                    var payouts = new Dictionary<int, decimal>
                    {
                        [3] = symbol!["Pay3"]?.GetValue<decimal>() ?? symbol!["Pay 3"]?.GetValue<decimal>() ?? 0.0m,
                        [4] = symbol!["Pay4"]?.GetValue<decimal>() ?? 0.0m,
                        [5] = symbol!["Pay5"]?.GetValue<decimal>() ?? 0.0m
                    };
                    paytable[symbolName] = payouts;
                }
            }

            var dto = new Data_LineBasedPayoutCalculator
            {
                Paytable = paytable.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (IReadOnlyDictionary<int, decimal>)kvp.Value
                )
            };

            return dto;
        }
    }
}
