using SlotLab.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlotLab.Engine.Core
{
    /// <summary>
    /// Evaluates a grid by counting occurrences of specific symbols and 
    /// returning their positions and totals.
    /// Useful for features like Scatter detection, Bonus symbol counting, etc.
    /// </summary>
    public class SymbolCountEvaluator_Default : IGridEvaluator<GridEvaluatorSymbolCountOutputRulesData>
    {
        /// <summary>
        /// List of symbol IDs to search for in the visible window.
        /// </summary>
        protected readonly List<string> symbolsToFind;

        public SymbolCountEvaluator_Default(List<string> symbolsToFind)
        {
            this.symbolsToFind = symbolsToFind;
        }

        public GridEvaluatorSymbolCountOutputRulesData Evaluate(List<List<string>> visibleWindow)
        {
            // Dictionary that stores all detected positions for each target symbol
            var detections = new Dictionary<string, List<(int row, int col)>>();

            // Initialize an empty list for each symbol we want to track
            foreach (var symbol in symbolsToFind)
            {
                detections[symbol] = new List<(int row, int col)>();
            }

            // Iterate over the entire grid [col][row]
            for (int col = 0; col < visibleWindow.Count; col++)
            {
                var column = visibleWindow[col];

                for (int row = 0; row < column.Count; row++)
                {
                    var cellSymbol = column[row];

                    // If the current cell's symbol matches one of the targets, record its position
                    if (symbolsToFind.Contains(cellSymbol, StringComparer.OrdinalIgnoreCase))
                    {
                        detections[cellSymbol].Add((row, col));
                    }
                }
            }

            // Return all found symbol positions and counts
            return new GridEvaluatorSymbolCountOutputRulesData(detections);
        }
    }
}
