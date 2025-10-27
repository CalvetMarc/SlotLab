using System.Text.Json.Nodes;

namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Immutable configuration DTO for line-based evaluation.
    /// Defines the paylines structure used by the evaluator.
    /// </summary>
    public sealed class Data_LineBasedEvaluator
    {
        // List of paylines. Each line is a list of row indices per column.
        public IReadOnlyList<IReadOnlyList<int>> Paylines { get; init; } = Array.Empty<IReadOnlyList<int>>();

        private Data_LineBasedEvaluator() { }

        public static Data_LineBasedEvaluator Load(JsonNode root)
        {
            if (root is null)
                throw new InvalidOperationException("JSON root is null.");

            var paylines = new List<IReadOnlyList<int>>();
            var paylinesNode = root["paylines"]?.AsArray();

            // Parse paylines from JSON
            if (paylinesNode != null)
            {
                foreach (var lineNode in paylinesNode)
                {
                    // Each payline is represented as an object with key-value pairs like:
                    // { "Reel1": 0, "Reel2": 1, "Reel3": 2 }
                    var line = lineNode!.AsObject()
                        .OrderBy(kv => kv.Key) // Ensure the reels are ordered (Reel1, Reel2, ...)
                        .Select(kv => kv.Value!.GetValue<int>())
                        .ToArray();

                    paylines.Add(line);
                }
            }

            // Create the immutable DTO instance
            return new Data_LineBasedEvaluator
            {
                Paylines = paylines
            };
        }
    }
}
