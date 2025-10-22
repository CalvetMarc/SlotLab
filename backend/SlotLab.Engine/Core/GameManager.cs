using System.Text.Json.Nodes;
using SlotLab.Engine.Core;

namespace SlotLab.Engine.Core
{
    public class GameManager
    {
        private readonly BaseGame baseGame;

        public GameManager(string configPath)
        {
            var configJson = File.ReadAllText(configPath);
            var jsonNode = JsonNode.Parse(configJson)!;

            string gameId = jsonNode["gameId"]!.ToString();
            int rows = jsonNode["rows"]!.GetValue<int>();
            int columns = jsonNode["columns"]!.GetValue<int>();

            string[][] strips = jsonNode["strips"]!.AsArray()
                .Select(reel => reel!.AsArray().Select(symbol => symbol!.ToString()).ToArray())
                .ToArray();

            baseGame = new BaseGame(gameId, rows, columns, strips);

            Console.WriteLine($"✅ Configuració carregada: {gameId} ({columns}x{rows})");
        }

        public object Spin()
        {
            return baseGame.Spin();
        }
    }
    
}