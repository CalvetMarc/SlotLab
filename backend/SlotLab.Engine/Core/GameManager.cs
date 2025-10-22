using System.Text.Json.Nodes;

namespace SlotLab.Engine.Core
{
    public class GameManager
    {
        //private readonly BaseGame baseGame;

        public GameManager(string configPath)
        {
            Rng.Initialize(); 

            var configJson = File.ReadAllText(configPath);
            var jsonNode = JsonNode.Parse(configJson)!;

            string gameId = jsonNode["gameId"]!.ToString();
            int rows = jsonNode["rows"]!.GetValue<int>();
            int columns = jsonNode["columns"]!.GetValue<int>();

            string[][] strips = jsonNode["strips"]!.AsArray().Select(reel => reel!.AsArray().Select(symbol => symbol!.ToString()).ToArray()).ToArray();
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

            //baseGame = new BaseGame(gameId, rows, columns, strips, paylines, paytable);

            Console.WriteLine($"✅ Configuració carregada: {gameId} ({columns}x{rows})");
            Console.WriteLine($"🎲 RNG PCG inicialitzat i llest per generar spins");
        }

        public object Spin()
        {
            // 🔹 Quan fem un spin, el BaseGame usarà el RNG natiu via Rng.Next()
            //return baseGame.Spin();
            return null;
        }
    }
}
