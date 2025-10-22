using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class BaseGame
    {
        private readonly string gameId;
        private readonly int rows;
        private readonly int columns;
        private readonly string[][] strips;

        public BaseGame(string gameId, int rows, int columns, string[][] strips)
        {
            this.gameId = gameId;
            this.rows = rows;
            this.columns = columns;
            this.strips = strips;
        }

        private string GetSymbol(string[] strip, int index)
            => strip[(index + strip.Length) % strip.Length];

        public SpinResult Spin()
        {
            var preRoll = new List<object>();
            var visibleWindow = new List<object>();
            var postRoll = new List<object>();

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

            return new SpinResult
            {
                PreRoll = preRoll,
                VisibleWindow = visibleWindow,
                PostRoll = postRoll,
                WinAmount = 0.0
            };
        }


    }
}