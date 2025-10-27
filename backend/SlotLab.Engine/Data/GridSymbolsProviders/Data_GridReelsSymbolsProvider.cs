using System.Text.Json.Nodes;

namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Config DTO for GridReelsSymbolsProvider (immutable).
    /// </summary>
    public sealed class Data_GridReelsSymbolsProvider
    {
        public int PreCount { get; init; } = 2;
        public int PostCount { get; init; } = 2;
        public int VisibleRows { get; init; }
        public IReadOnlyList<IReadOnlyList<string>> GameStrips { get; init; } = Array.Empty<IReadOnlyList<string>>();

        private Data_GridReelsSymbolsProvider() { }

        public static Data_GridReelsSymbolsProvider Load(JsonNode root)
        {
            if (root is null)
                throw new InvalidOperationException("JSON root is null.");

            // GRID
            var gridNode = root["grid"]?.AsArray()?.FirstOrDefault()
                ?? throw new InvalidOperationException("Missing 'grid' section.");
            int rows = gridNode["Rows"]?.GetValue<int>() 
                ?? throw new InvalidOperationException("Missing 'Rows' in grid.");
            int columns = gridNode["Columns"]?.GetValue<int>() 
                ?? throw new InvalidOperationException("Missing 'Columns' in grid.");

            // OPTIONAL pre/post
            int pre = gridNode["PreCount"]?.GetValue<int>() ?? 2;
            int post = gridNode["PostCount"]?.GetValue<int>() ?? 2;

            // STRIPS
            var stripsNode = root["strips"]?.AsObject()
                ?? throw new InvalidOperationException("Missing 'strips' section.");
            var strips = new List<IReadOnlyList<string>>();
            foreach (var strip in stripsNode.OrderBy(kv => kv.Key))
            {
                var values = strip.Value!.AsArray().Select(v => v!.ToString()).ToList();
                if (values.Count == 0) throw new InvalidOperationException($"Empty strip: '{strip.Key}'.");
                strips.Add(values);
            }

            if (strips.Count != columns)
                throw new InvalidOperationException($"Strips count ({strips.Count}) != Columns ({columns}).");

            var dto = new Data_GridReelsSymbolsProvider
            {
                VisibleRows = rows,
                GameStrips = strips,
                PreCount = pre,
                PostCount = post
            };

            // Extra validacions
            if (dto.VisibleRows <= 0) throw new InvalidOperationException("VisibleRows must be > 0.");
            return dto;
        }
    }
}
