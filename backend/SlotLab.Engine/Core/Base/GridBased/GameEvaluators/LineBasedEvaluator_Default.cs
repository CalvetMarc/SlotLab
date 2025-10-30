using SlotLab.Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Evaluates paylines and detects winning symbol combinations.
    /// NOTE: This class does NOT calculate payouts — only identifies winning lines.
    /// </summary>
    public class LineBasedEvaluator_Default : IGridEvaluator<GridEvaluatorLineBasedOutputRulesData>
    {
        // Configuration data (contains paylines only)
        protected readonly Data_LineBasedEvaluator lineBasedEvaluatorData;

        public LineBasedEvaluator_Default(Data_LineBasedEvaluator lineBasedEvaluatorData)
        {
            this.lineBasedEvaluatorData = lineBasedEvaluatorData;
        }

        /// <summary>
        /// Evaluates visible symbols on the reels and returns all winning lines.
        /// </summary>
        /// <param name="visibleWindow">
        /// A 2D list of visible symbols [column][row] representing the reel window.
        /// </param>
        /// <returns>Structured information about all detected winning lines.</returns>
        public GridEvaluatorLineBasedOutputRulesData Evaluate(IReadOnlyList<IReadOnlyList<string>> visibleWindow)
        {
            // Stores all found winning lines (line number, base symbol, match count)
            var winningLines = new List<(int lineNumber, string symbol, int lineSymbolsCount)>();

            // Iterate through every configured payline
            for (int lineIndex = 0; lineIndex < lineBasedEvaluatorData.Paylines.Count; lineIndex++)
            {
                var line = lineBasedEvaluatorData.Paylines[lineIndex];
                var symbols = new List<string>();

                // Collect the visible symbols along this payline
                for (int col = 0; col < visibleWindow.Count; col++)
                {
                    int row = line[col];                // Row index for this reel according to the line definition
                    symbols.Add(visibleWindow[col][row]); // Symbol at [column][row]
                }

                // Find the first "base" symbol (skip Wilds and Scatters)
                string? firstSymbol = symbols.FirstOrDefault(s => s != "Wild" && s != "Scatter");
                if (string.IsNullOrEmpty(firstSymbol))
                    continue; // No valid base symbol found, skip this line

                // Count how many consecutive symbols (from the left) match the base symbol or are Wilds
                int count = 0;
                foreach (var sym in symbols)
                {
                    if (sym == firstSymbol || sym == "Wild")
                        count++;
                    else
                        break;
                }

                // Only consider it a winning line if at least 3 symbols match
                if (count >= 3)
                    winningLines.Add((lineIndex, firstSymbol, count));
            }

            // Return all detected winning lines (no payout calculation)
            return new GridEvaluatorLineBasedOutputRulesData(winningLines);
        }
    }
}
