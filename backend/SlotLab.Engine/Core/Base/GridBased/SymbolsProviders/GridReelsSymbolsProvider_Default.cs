using System.Text.Json.Nodes;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class GridReelsSymbolsProvider_Default : ISymbolsProvider
    {
        private readonly int preCount, postCount;
        private readonly List<List<string>> gameStrips;
        private readonly int visibleRows;

        public GridReelsSymbolsProvider_Default(List<List<string>> gameStrips, int visibleRows, int preCount = 2, int postCount = 2)
        {
            this.visibleRows = visibleRows;
            this.preCount = preCount;
            this.postCount = postCount;
            this.gameStrips = gameStrips;
        }

        public SpinResultData Spin()
        {
            var visibleWindow = new List<List<string>>();
            var preRoll = new List<List<string>>();
            var postRoll = new List<List<string>>();

            for (int reelIndex = 0; reelIndex < gameStrips.Count; reelIndex++)
            {
                var reel = gameStrips[reelIndex];
                int stopIndex = Rng.NextIntBetween(0, reel.Count - 1);
                // --- PreRoll ---
                var pre = new List<string>();
                for (int i = preCount; i > 0; i--)
                    pre.Add(GetSymbolCircular(reel, stopIndex - i));

                // --- Visible ---
                var visible = new List<string>();
                for (int i = 0; i < visibleRows; i++)
                    visible.Add(GetSymbolCircular(reel, stopIndex + i));

                // --- PostRoll ---
                var post = new List<string>();
                for (int i = 0; i < postCount; i++)
                    post.Add(GetSymbolCircular(reel, stopIndex + visibleRows + i));

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

        string GetSymbolCircular(List<string> reel, int index)
        {
            if (reel == null || reel.Count == 0)
                throw new ArgumentException("Reel cannot be null or empty", nameof(reel));

            int normalizedIndex = ((index % reel.Count) + reel.Count) % reel.Count;
            return reel[normalizedIndex];
        }        
        
    }
}
