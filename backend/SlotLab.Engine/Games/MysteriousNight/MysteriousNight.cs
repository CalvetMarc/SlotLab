using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using System.Text.Json.Nodes;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public class MysteriousNight
    {
        public readonly GameBase gameBase;

        public MysteriousNight(JsonNode jsonNode)
        {
            Rng.Initialize();

            var gridNode = jsonNode["grid"]?.AsArray()?.FirstOrDefault();
            int rows = gridNode?["Rows"]?.GetValue<int>() ?? 0;
            int columns = gridNode?["Columns"]?.GetValue<int>() ?? 0;

            var stripsNode = jsonNode["strips"]?.AsObject();
            List<List<string>> strips = new();
            if (stripsNode != null)
            {
                foreach (var strip in stripsNode.OrderBy(kv => kv.Key))
                {
                    var values = strip.Value!.AsArray().Select(v => v!.ToString()).ToList();
                    strips.Add(values);
                }
            }

            var paylinesNode = jsonNode["paylines"]?.AsArray();
            List<int[]> paylines = new();
            if (paylinesNode != null)
            {
                foreach (var lineNode in paylinesNode)
                {
                    var line = lineNode!.AsObject()
                        .OrderBy(kv => kv.Key) // Ordenem "Reel 1", "Reel 2", etc.
                        .Select(kv => kv.Value!.GetValue<int>())
                        .ToArray();
                    paylines.Add(line);
                }
            }

            var paytableNode = jsonNode["paytable"]?.AsArray();
            var paytable = new Dictionary<string, Dictionary<int, double>>();
            if (paytableNode != null)
            {
                foreach (var symbol in paytableNode)
                {
                    string symbolName = symbol!["Symbol"]!.GetValue<string>();
                    var payouts = new Dictionary<int, double>
                    {
                        [3] = symbol!["Pay 3"]?.GetValue<double>() ?? 0.0,
                        [4] = symbol!["Pay4"]?.GetValue<double>() ?? 0.0,
                        [5] = symbol!["Pay5"]?.GetValue<double>() ?? 0.0
                    };
                    paytable[symbolName] = payouts;
                }
            }

            gameBase = new GameBase( new ReelsSymbolsProvider_Default(strips, rows), new LineBasedEvaluator_Default(paylines, paytable));
        }
    }
}
