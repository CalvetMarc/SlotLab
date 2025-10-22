using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using System.Text.Json.Nodes;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public class MysteriousNight
    {
        public readonly GameBase gameBase;

        public MysteriousNight(string configPath)
        {
            Rng.Initialize();

            var configJson = File.ReadAllText(configPath);
            var jsonNode = JsonNode.Parse(configJson)!;

            string gameId = jsonNode["gameId"]!.ToString();
            int rows = jsonNode["rows"]!.GetValue<int>();
            int columns = jsonNode["columns"]!.GetValue<int>();

            List<List<string>> strips = jsonNode["strips"]!.AsArray().Select(reel => reel!.AsArray().Select(symbol => symbol!.ToString()).ToList()).ToList();
            var paylines = jsonNode["paylines"]!.AsArray().Select(line => line!.AsArray().Select(n => n!.GetValue<int>()).ToArray()).ToList();

            var paytable = new Dictionary<string, Dictionary<int, double>>();
            var paytableNode = jsonNode["paytable"]!.AsObject();

            foreach (var symbolEntry in paytableNode)
            {
                var payouts = new Dictionary<int, double>();
                foreach (var kv in symbolEntry.Value!.AsObject())
                {
                    // Ex: key = "x3" → count = 3
                    int count = int.Parse(kv.Key.TrimStart('x'));
                    double value = kv.Value!.GetValue<double>();
                    payouts[count] = value;
                }
                paytable[symbolEntry.Key] = payouts;
            }

            gameBase = new GameBase(new ReelsSymbolsProvider_Default(strips, rows), new LineBasedEvaluator_Default(paylines, paytable));
        }

    }
}
