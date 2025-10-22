using System;
using System.Collections.Generic;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core
{
    public class BaseGame
    {
        private readonly string gameId;
        private readonly int rows;
        private readonly int columns;
        private readonly string[][] strips;
        private readonly Random rng;

        public BaseGame(string gameId, int rows, int columns, string[][] strips)
        {
            this.gameId = gameId;
            this.rows = rows;
            this.columns = columns;
            this.strips = strips;
            this.rng = new Random();
        }

        // 🔹 Funció auxiliar: accés circular a símbols
        private string GetSymbol(string[] strip, int index)
            => strip[(index + strip.Length) % strip.Length];

        // 🔹 Genera una tirada (spin) i retorna un SpinResult
        public SpinResult Spin()
        {
            var reelsResult = new List<object>();

            for (int reelIndex = 0; reelIndex < strips.Length; reelIndex++)
            {
                var strip = strips[reelIndex];
                int stopIndex = rng.Next(strip.Length);

                var visibleWindow = new[]
                {
                    GetSymbol(strip, stopIndex - 1),
                    GetSymbol(strip, stopIndex),
                    GetSymbol(strip, stopIndex + 1)
                };

                var preRoll = new[]
                {
                    GetSymbol(strip, stopIndex - 3),
                    GetSymbol(strip, stopIndex - 2)
                };

                var postRoll = new[]
                {
                    GetSymbol(strip, stopIndex + 2),
                    GetSymbol(strip, stopIndex + 3)
                };

                reelsResult.Add(new
                {
                    visibleWindow,
                    preRoll,
                    postRoll
                });
            }

            // Retornem un objecte SpinResult tipat
            return new SpinResult
            {
                Game = gameId,
                Rows = rows,
                Columns = columns,
                Reels = reelsResult,
                WinAmount = 0.0,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
