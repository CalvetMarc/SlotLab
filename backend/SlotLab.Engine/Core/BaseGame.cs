using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class BaseGame
    {
        private readonly string gameId;
        private readonly int rows;
        private readonly int columns;
        private readonly string[][] strips;

        public BaseGame(string gameId, int rows, int columns, string[][] strips)
        {
            this.gameId = gameId;
            this.rows = rows;
            this.columns = columns;
            this.strips = strips;
        }

        private string GetSymbol(string[] strip, int index)
            => strip[(index + strip.Length) % strip.Length];

        public SpinResult Spin()
        {
            var reelsResult = new List<object>();

            for (int reelIndex = 0; reelIndex < strips.Length; reelIndex++)
            {
                var strip = strips[reelIndex];

                int stopIndex = Rng.NextIntBetween(0, strip.Length - 1);

                var visibleWindow = new[]
                {
                    GetSymbol(strip, stopIndex - 1),
                    GetSymbol(strip, stopIndex),
                    GetSymbol(strip, stopIndex + 1)
                };

                reelsResult.Add(new
                {
                    visibleWindow
                });
            }

            return new SpinResult
            {
                Game = gameId,
                Rows = rows,
                Columns = columns,
                Reels = reelsResult,
                WinAmount = 0.0,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
