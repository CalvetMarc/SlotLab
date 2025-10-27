using SlotLab.Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Evaluates paylines and produces structured information about winning lines.
    /// (Does NOT calculate payouts — that’s handled separately.)
    /// </summary>
    public class LineBasedEvaluator_Default : IGridEvaluator<GridEvaluatorLineBasedOutputRulesData>
    {
        protected readonly List<int[]> paylines;
        protected readonly Dictionary<string, Dictionary<int, double>> paytable;

        public LineBasedEvaluator_Default(
            List<int[]> paylines,
            Dictionary<string, Dictionary<int, double>> paytable)
        {
            this.paylines = paylines;
            this.paytable = paytable;
        }

        public GridEvaluatorLineBasedOutputRulesData Evaluate(List<List<string>> visibleWindow)
        {
            // Aquesta llista guardarà les línies guanyadores
            var winningLines = new List<(int lineNumber, string symbol, int lineSymbolsCount)>();

            // Recorrem totes les línies configurades
            for (int lineIndex = 0; lineIndex < paylines.Count; lineIndex++)
            {
                var line = paylines[lineIndex];
                var symbols = new List<string>();

                // Obtenim els símbols visibles d’aquesta línia
                for (int col = 0; col < visibleWindow.Count; col++)
                {
                    int row = line[col];
                    symbols.Add(visibleWindow[col][row]);
                }

                // Troba el primer símbol “base” (que no sigui Wild ni Scatter)
                string? firstSymbol = symbols.FirstOrDefault(s => s != "Wild" && s != "Scatter");
                if (string.IsNullOrEmpty(firstSymbol) || !paytable.ContainsKey(firstSymbol))
                    continue;

                // Comptem quants símbols consecutius (iguals o Wilds) hi ha des de l’esquerra
                int count = 0;
                foreach (var sym in symbols)
                {
                    if (sym == firstSymbol || sym == "Wild")
                        count++;
                    else
                        break;
                }

                // Si la línia compleix el mínim de coincidències, la guardem
                if (count >= 3)
                {
                    winningLines.Add((lineIndex, firstSymbol, count));
                }
            }

            // Retornem totes les línies guanyadores trobades
            return new GridEvaluatorLineBasedOutputRulesData(winningLines);
        }
    }
}
