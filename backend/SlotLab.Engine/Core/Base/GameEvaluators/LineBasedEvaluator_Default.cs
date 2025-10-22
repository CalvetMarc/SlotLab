namespace SlotLab.Engine.Core
{
    public class LineBasedEvaluator_Default : IGameEvaluator
    {
        private readonly List<int[]> paylines;
        private readonly Dictionary<string, Dictionary<int, double>> paytable;

        public LineBasedEvaluator_Default(List<int[]> paylines, Dictionary<string, Dictionary<int, double>> paytable)
        {
            this.paylines = paylines;
            this.paytable = paytable;
        }

        public double Evaluate(List<List<string>> visibleWindow, double bet = 1.0)
        {
            double totalWin = 0.0;

            foreach (var line in paylines)
            {
                // Get the symbols on this payline
                var symbols = new List<string>();
                for (int col = 0; col < visibleWindow.Count; col++)
                {
                    int row = line[col];
                    symbols.Add(visibleWindow[col][row]);
                }

                // Find the first non-Wild and non-Scatter symbol
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

                // If 3 or more consecutive matches, apply payout
                if (count >= 3 && paytable[firstSymbol].ContainsKey(count))
                {
                    double payout = paytable[firstSymbol][count];
                    totalWin += payout * bet;
                }
            }

            return totalWin;
        }
    }
}
