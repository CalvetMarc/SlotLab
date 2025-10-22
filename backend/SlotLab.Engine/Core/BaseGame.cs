using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class BaseGame
    {
        private readonly string gameId;
        private readonly int rows;
        private readonly int columns;
        private readonly string[][] strips;
        private readonly List<int[]> paylines;   
        private readonly Dictionary<string, Dictionary<int, double>> paytable; 

        public BaseGame(string gameId, int rows, int columns, string[][] strips, List<int[]> paylines, Dictionary<string, Dictionary<int, double>> paytable)
        {
            this.gameId = gameId;
            this.rows = rows;
            this.columns = columns;
            this.strips = strips;
            this.paylines = paylines;
            this.paytable = paytable;
        }

        private string GetSymbol(string[] strip, int index)
            => strip[(index + strip.Length) % strip.Length];

        public SpinResult Spin()
        {
            var preRoll = new List<List<string>>();
            var visibleWindow = new List<List<string>>();
            var postRoll = new List<List<string>>();

            for (int reelIndex = 0; reelIndex < columns; reelIndex++)
            {
                var reel = strips[reelIndex];
                int stopIndex = Rng.NextIntBetween(0, reel.Length - 1);

                int preCount = 2;
                var pre = new List<string>();
                for (int i = preCount; i > 0; i--)
                    pre.Add(GetSymbol(reel, stopIndex - i));

                var visible = new List<string>();
                for (int i = 0; i < rows; i++)
                    visible.Add(GetSymbol(reel, stopIndex + i));

                int postCount = 2;
                var post = new List<string>();
                for (int i = 0; i < postCount; i++)
                    post.Add(GetSymbol(reel, stopIndex + columns + i));

                preRoll.Add(pre);
                visibleWindow.Add(visible);
                postRoll.Add(post);
            }

            double totalWin = EvaluateSpin(visibleWindow);

            return new SpinResult
            {
                PreRoll = preRoll,
                VisibleWindow = visibleWindow,
                PostRoll = postRoll,
                WinAmount = totalWin
            };
        }

        private double EvaluateSpin(List<List<string>> visibleWindow, double bet = 1.0)
        {
            double totalWin = 0.0;

            foreach (var line in paylines)
            {
                // Get the symbols of the current payline
                var symbols = new List<string>();
                for (int col = 0; col < columns; col++)
                {
                    int row = line[col];
                    symbols.Add(visibleWindow[col][row]);
                }

                // Find the first valid symbol (not Wild or Scatter)
                string? firstSymbol = symbols.FirstOrDefault(s => s != "Wild" && s != "Scatter");
                if (string.IsNullOrEmpty(firstSymbol) || !paytable.ContainsKey(firstSymbol))
                    continue;

                // Count consecutive matches from the left
                int count = 0;
                foreach (var sym in symbols)
                {
                    if (sym == firstSymbol || sym == "Wild")
                        count++;
                    else
                        break;
                }

                // If there are 3 or more consecutive matches, calculate the payout
                if (count >= 3 && paytable[firstSymbol].ContainsKey(count))
                {
                    double payout = paytable[firstSymbol][count];
                    double winAmount = payout * bet;
                    totalWin += winAmount;
                }
            }

            return totalWin;
        }


    }
}