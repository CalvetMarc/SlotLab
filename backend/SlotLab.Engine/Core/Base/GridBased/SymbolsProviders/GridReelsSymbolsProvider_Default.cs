using System.Text.Json.Nodes;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class GridReelsSymbolsProvider_Default : IGridSymbolsProvider
    {
        private readonly Data_GridReelsSymbolsProvider gridReelsSymbolsProviderData;

        public GridReelsSymbolsProvider_Default(Data_GridReelsSymbolsProvider gridReelsSymbolsProviderData)
        {
            this.gridReelsSymbolsProviderData = gridReelsSymbolsProviderData;
        }

        public SpinResultData Spin()
        {
            var visibleWindow = new List<List<string>>();
            var preRoll = new List<List<string>>();
            var postRoll = new List<List<string>>();

            for (int reelIndex = 0; reelIndex < gridReelsSymbolsProviderData.GameStrips.Count; reelIndex++)
            {
                var reel = gridReelsSymbolsProviderData.GameStrips[reelIndex];
                int stopIndex = Rng.NextIntBetween(0, reel.Count - 1);
                // --- PreRoll ---
                var pre = new List<string>();
                for (int i = gridReelsSymbolsProviderData.PreCount; i > 0; i--)
                    pre.Add(GetSymbolCircular(reel, stopIndex - i));

                // --- Visible ---
                var visible = new List<string>();
                for (int i = 0; i < gridReelsSymbolsProviderData.VisibleRows; i++)
                    visible.Add(GetSymbolCircular(reel, stopIndex + i));

                // --- PostRoll ---
                var post = new List<string>();
                for (int i = 0; i < gridReelsSymbolsProviderData.PostCount; i++)
                    post.Add(GetSymbolCircular(reel, stopIndex + gridReelsSymbolsProviderData.VisibleRows + i));

                preRoll.Add(pre);
                visibleWindow.Add(visible);
                postRoll.Add(post);
            }

            return new SpinResultData
            {
                VisibleWindow = visibleWindow,
                Metadata = new Dictionary<string, object>
                {
                    ["PreRoll"] = preRoll,
                    ["PostRoll"] = postRoll
                }
            };
        }

        string GetSymbolCircular(IReadOnlyList<string> reel, int index)
        {
            if (reel == null || reel.Count == 0)
                throw new ArgumentException("Reel cannot be null or empty", nameof(reel));

            int normalizedIndex = ((index % reel.Count) + reel.Count) % reel.Count;
            return reel[normalizedIndex];
        }        
        
    }
}
