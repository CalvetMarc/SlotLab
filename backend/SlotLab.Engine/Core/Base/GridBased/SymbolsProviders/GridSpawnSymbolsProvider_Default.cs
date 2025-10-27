using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class GridSpawnSymbolsProvider_Default : IGridSymbolsProvider
    {
        private readonly List<List<string>> gameReels;
        private readonly List<KeyValuePair<string, Dictionary<string, double>>> spawnerTable;

        public GridSpawnSymbolsProvider_Default(int columns, int rows, List<KeyValuePair<string, Dictionary<string, double>>> spawnerTable)
        {
            gameReels = new List<List<string>>();
            for (int i = 0; i < columns; i++)
            {
                var reel = new List<string>();
                for (int j = 0; j < rows; j++)
                    reel.Add(string.Empty);

                gameReels.Add(reel);
            }
            
            this.spawnerTable = spawnerTable;
        }

        public SpinResultData Spin()
        {
            int rows = gameReels.FirstOrDefault()?.Count ?? 0;
            int cols = gameReels.Count;

            // Inicialitzem la grid buida
            var grid = new List<List<string>>();
            for (int r = 0; r < rows; r++)
                grid.Add(Enumerable.Repeat("Empty", cols).ToList());

            // Apliquem cada capa de spawn successivament
            foreach (var layer in spawnerTable)
            {
                string triggerSymbol = layer.Key; // "" = aplicar a totes
                var table = layer.Value;

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        string currentSymbol = grid[row][col];

                        // Si la capa no aplica a aquesta cella, saltem
                        if (!string.IsNullOrEmpty(triggerSymbol) &&
                            !currentSymbol.Equals(triggerSymbol, StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Fem el roll RNG per a aquesta taula
                        double roll = Rng.NextFloatBetween(0.0, 100.0);
                        double cumulative = 0.0;
                        string? newSymbol = null;

                        foreach (var kv in table)
                        {
                            cumulative += kv.Value;
                            if (roll <= cumulative)
                            {
                                newSymbol = kv.Key;
                                break;
                            }
                        }

                        newSymbol ??= "Empty";
                        grid[row][col] = newSymbol;
                    }
                }
            }

            return new SpinResultData
            {
                VisibleWindow = gameReels
            };
        }
    }
}
