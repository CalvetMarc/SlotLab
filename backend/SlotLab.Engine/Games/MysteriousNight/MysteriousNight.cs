using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using System.Text.Json.Nodes;
using SlotLab.Engine.Models;

namespace SlotLab.Engine.Games
{
    public class MysteriousNight
    {
        private PlayResultData lastBaseResult;
        private readonly GameBase gameBase;
        private readonly GameBonus gameBonus;

        List<(int Level, int Scatters, int StartFreeSpins, int BonusToUpgrade)> levels;


        public MysteriousNight(JsonNode jsonNode)
        {
            Rng.Initialize();
            gameBase = CreateGameBase(jsonNode) ?? throw new InvalidOperationException("Failed to create GameBase");
        }

        /// <summary>
        /// Builds and returns the GameBase instance from the JSON configuration.
        /// Reads the base section of the configuration file, extracting grid, strips,
        /// paylines, and paytable data.
        /// </summary>
        /// <param name="jsonNode">The JSON node containing the game configuration.</param>
        /// <returns>A fully configured GameBase instance.</returns>
        private static GameBase? CreateGameBase(JsonNode jsonNode)
        {
            var baseNode = jsonNode;
            if (baseNode == null)
                throw new InvalidOperationException("Missing 'base' section in JSON configuration.");

            // --- GRID ---
            var gridNode = baseNode["grid"]?.AsArray()?.FirstOrDefault();
            int rows = gridNode?["Rows"]?.GetValue<int>() ?? 0;
            int columns = gridNode?["Columns"]?.GetValue<int>() ?? 0;

            // --- STRIPS ---
            var strips = new List<List<string>>();
            var stripsNode = baseNode["strips"]?.AsObject();
            if (stripsNode != null)
            {
                foreach (var strip in stripsNode.OrderBy(kv => kv.Key))
                {
                    var values = strip.Value!.AsArray()
                        .Select(v => v!.ToString())
                        .ToList();
                    strips.Add(values);
                }
            }

            // --- PAYLINES ---
            var paylines = new List<int[]>();
            var paylinesNode = baseNode["paylines"]?.AsArray();
            if (paylinesNode != null)
            {
                foreach (var lineNode in paylinesNode)
                {
                    var line = lineNode!.AsObject()
                        .OrderBy(kv => kv.Key) // Ordenem "Reel1", "Reel2", etc.
                        .Select(kv => kv.Value!.GetValue<int>())
                        .ToArray();
                    paylines.Add(line);
                }
            }

            // --- PAYTABLE ---
            var paytable = new Dictionary<string, Dictionary<int, double>>();
            var paytableNode = baseNode["paytable"]?.AsArray();
            if (paytableNode != null)
            {
                foreach (var symbol in paytableNode)
                {
                    string symbolName = symbol!["Symbol"]!.GetValue<string>();
                    var payouts = new Dictionary<int, double>
                    {
                        [3] = symbol!["Pay3"]?.GetValue<double>() ?? symbol!["Pay 3"]?.GetValue<double>() ?? 0.0,
                        [4] = symbol!["Pay4"]?.GetValue<double>() ?? 0.0,
                        [5] = symbol!["Pay5"]?.GetValue<double>() ?? 0.0
                    };
                    paytable[symbolName] = payouts;
                }
            }

            // --- CREATE GAME BASE ---
            return new GameBase(
                new ReelsSymbolsProvider_Default(strips, rows),
                new LineBasedEvaluator_Default(paylines, paytable)
            );
        }

        private static GameBonus? CreateGameBonus(JsonNode jsonNode)
        {
            var baseNode = jsonNode;
            if (baseNode == null)
                throw new InvalidOperationException("Missing 'base' section in JSON configuration.");

            // --- BONUS_SPAWNER_PROBABILITIES ---
            var bonusSpawnerProbabilitiesNode = baseNode["bonus_spawner_probabilites"]?.AsArray();
            Dictionary<string, double> bonusSpawnerProbabilities = new();

            if (bonusSpawnerProbabilitiesNode != null)
            {
                // Si hi ha almenys un element, agafem el primer (la taula de probabilitats)
                var firstProbabilityGroup = bonusSpawnerProbabilitiesNode.FirstOrDefault()?.AsObject();

                if (firstProbabilityGroup != null)
                {
                    bonusSpawnerProbabilities = firstProbabilityGroup.ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value!.GetValue<double>()
                    );
                }
            }

            // --- CARD_MULTIPLIER_SPAWNER ---
            var cardMultiplierSpawnerNode = baseNode["card_multiplier_spawner"]?.AsArray();
            List<(int multiplier, double probability)> cardMultiplierSpawner = new();
            if (cardMultiplierSpawnerNode != null)
            {
                foreach (var entry in cardMultiplierSpawnerNode)
                {
                    int multiplier = entry!["Card Multiplier"]?.GetValue<int>() ?? 0;
                    double probability = entry!["Probaility %"]?.GetValue<double>() ?? 0.0;
                    cardMultiplierSpawner.Add((multiplier, probability));
                }
            }

            // --- LEVELS ---
            var levelsNode = baseNode["levels"]?.AsArray();
            List<(int Level, int Scatters, int StartFreeSpins, int BonusToUpgrade)> levels = new();
            if (levelsNode != null)
            {
                foreach (var level in levelsNode)
                {
                    levels.Add((
                        Level: level!["Level"]?.GetValue<int>() ?? 0,
                        Scatters: level!["Scatter"]?.GetValue<int>() ?? 0,
                        StartFreeSpins: level!["Start Free Spins"]?.GetValue<int>() ?? 0,
                        BonusToUpgrade: level!["Bonus to upgrade"]?.GetValue<int>() ?? 0
                    ));
                }
            }

            // --- CREATE GAME BASE ---
            //return new GameBonus(new ScatterBonusTrigger_Default(levels.First().Scatters), null, null);ç
            return null;
        }


        public PlayResultData PlayBase(double bet = 1.0) 
        {
            lastBaseResult = gameBase.Play(bet);
            return lastBaseResult;
        }

        public bool CanPlayBonus()
        {
            (bool, Dictionary<string, object>?) bonusStartData = gameBonus.CanBonusStart(new Dictionary<string, object> { { "GameReels", lastBaseResult.SpinResultData.VisibleWindow } });

            if (bonusStartData.Item1)
            {
                int scatterCount = 0;
                if (bonusStartData.Item2 != null && bonusStartData.Item2.TryGetValue("Scatter", out var scatterValue))
                {
                    if (scatterValue is int value)
                        scatterCount = value;
                    else
                        scatterCount = Convert.ToInt32(scatterValue);
                }

                int startSpins = levels.First(l => l.Scatters == scatterCount).StartFreeSpins;
                gameBonus.StartBonus(new Dictionary<string, object> { { "Spins", startSpins } });
            }

            return bonusStartData.Item1;
        }

        public void PlayBonus()
        {
            gameBonus.TickBonus(null);

            //falta afegir un IGameMechanic que fara el que fa ISymbolProvider de Spawn, pero ha de poder ser mes felxible
            //a partir del IGameMechanic sha de poder acumular els multipliers i bonus symbols per sumar freespins al IBonusStateHandler mitjançant la metadata del tick
            //a partir del IGameMechanic sha de poder acumular els bonus symbols per sumar freespins al IBonusStateHandler mitjançant la metadata del tick
        }
        
        public bool HasBonusFinished(Dictionary<string, object>? metadata) => gameBonus.HasBonusFinished(metadata);
    }
}
