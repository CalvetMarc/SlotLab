using System.Text.Json.Nodes;

namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Config DTO for GridReelsSymbolsProvider (immutable).
    /// </summary>
    public sealed class Data_LineBasedPayoutCalculator
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<int, double>> Paytable { get; init; } = new Dictionary<string, IReadOnlyDictionary<int, double>>();       

        private Data_LineBasedPayoutCalculator() { }

        public static Data_LineBasedPayoutCalculator Load(JsonNode root)
        {
            if (root is null)
                throw new InvalidOperationException("JSON root is null.");           

            // --- PAYTABLE ---
            var paytable = new Dictionary<string, Dictionary<int, double>>();
            var paytableNode = root["paytable"]?.AsArray();
            if (paytableNode != null)
            {
                foreach (var symbol in paytableNode)
                {
                    string symbolName = symbol!["Symbol"]!.GetValue<string>();
                    var payouts = new Dictionary<int, double>
                    {
                        [3] = symbol!["Pay3"]?.GetValue<double>() ?? symbol!["Pay 3"]?.GetValue<double>() ?? 0.0,
                        [4] = symbol!["Pay4"]?.GetValue<double>() ?? 0.0,
                        [5] = symbol!["Pay5"]?.GetValue<double>() ?? 0.0
                    };
                    paytable[symbolName] = payouts;
                }
            }

            var dto = new Data_LineBasedPayoutCalculator
            {
                Paytable = paytable.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (IReadOnlyDictionary<int, double>)kvp.Value
                )
            };

            return dto;
        }
    }
}
