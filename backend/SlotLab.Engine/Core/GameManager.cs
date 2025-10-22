using System.Text.Json.Nodes;

namespace SlotLab.Engine.Core
{
    public class GameManager
    {
        private readonly BaseGame baseGame;

        public GameManager(string configPath)
        {
            Rng.Initialize(); 

            // 🔹 Carreguem configuració del joc
            var configJson = File.ReadAllText(configPath);
            var jsonNode = JsonNode.Parse(configJson)!;

            string gameId = jsonNode["gameId"]!.ToString();
            int rows = jsonNode["rows"]!.GetValue<int>();
            int columns = jsonNode["columns"]!.GetValue<int>();

            string[][] strips = jsonNode["strips"]!.AsArray()
                .Select(reel => reel!.AsArray().Select(symbol => symbol!.ToString()).ToArray())
                .ToArray();

            // 🔹 Creem el joc base
            baseGame = new BaseGame(gameId, rows, columns, strips);

            Console.WriteLine($"✅ Configuració carregada: {gameId} ({columns}x{rows})");
            Console.WriteLine($"🎲 RNG PCG inicialitzat i llest per generar spins");
        }

        public object Spin()
        {
            // 🔹 Quan fem un spin, el BaseGame usarà el RNG natiu via Rng.Next()
            return baseGame.Spin();
        }
    }
}
