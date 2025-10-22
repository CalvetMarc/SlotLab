using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class ReelsSymbolsProvider_Default : ISymbolsProvider
    {
        private readonly int visibleRows;
        private readonly int preCount;
        private readonly int postCount;
        protected ISymbolsProvider Super => ((ISymbolsProvider)this);


        public ReelsSymbolsProvider_Default(int visibleRows, int preCount = 2, int postCount = 2)
        {
            this.visibleRows = visibleRows;
            this.preCount = preCount;
            this.postCount = postCount;
        }

        public SpinResultData Spin(List<List<string>> gameStrips)
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
                    pre.Add(Super.GetSymbolCircular(reel, stopIndex - i));

                // --- Visible ---
                var visible = new List<string>();
                for (int i = 0; i < visibleRows; i++)
                    visible.Add(Super.GetSymbolCircular(reel, stopIndex + i));

                // --- PostRoll ---
                var post = new List<string>();
                for (int i = 0; i < postCount; i++)
                    post.Add(Super.GetSymbolCircular(reel, stopIndex + visibleRows + i));

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
    }
}
