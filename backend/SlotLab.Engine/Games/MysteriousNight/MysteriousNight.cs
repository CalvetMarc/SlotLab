using SlotLab.Engine.Core.Base;
using SlotLab.Engine.Core;
using SlotLab.Engine.Models;
using System.Text.Json.Nodes;

namespace SlotLab.Engine.Games
{
    /// <summary>
    /// Game state machine for the "MysteriousNight" slot.
    /// Declares the flow with a route table: (State, Trigger) -> Next State factory.
    /// </summary>
    public sealed class MysteriousNight : BaseGame<GridEvaluatorLineBasedOutputRulesData>
    {
        public MysteriousNight(JsonNode jsonNode, GameEnvironmentMode gameEnvironmentMode, ulong? seed = null) : base(
            new GridBasedGameMechanic_Default<GridEvaluatorLineBasedOutputRulesData>(
                new GridReelsSymbolsProvider_Default(Data_GridReelsSymbolsProvider.Load(jsonNode["base"]!)),
                new LineBasedEvaluator_Default(Data_LineBasedEvaluator.Load(jsonNode["base"]!)),
                new LineBasedPayoutCalculator_Default(Data_LineBasedPayoutCalculator.Load(jsonNode["base"]!))
            ), gameEnvironmentMode, seed)
        {
            slotStateFactory = new MysteriousNightBaseStateFactory<GridEvaluatorLineBasedOutputRulesData>();
        }
    }
}




/*
        private readonly GridBasedGameMechanic_Default<GridEvaluatorLineBasedOutputRulesData> gameBase;
        
        public MysteriousNight(JsonNode jsonNode)
        {
            Rng.Initialize();

            gameBase = new GridBasedGameMechanic_Default<GridEvaluatorLineBasedOutputRulesData>(
                new GridReelsSymbolsProvider_Default(Data_GridReelsSymbolsProvider.Load(jsonNode)),
                new LineBasedEvaluator_Default(Data_LineBasedEvaluator.Load(jsonNode)),
                new LineBasedPayoutCalculator_Default(Data_LineBasedPayoutCalculator.Load(jsonNode))
            );
        }



            var baseNode = jsonNode;
            if (baseNode == null)
                throw new InvalidOperationException("Missing 'base' section in JSON configuration.");

            var gridNode = baseNode["grid"]?.AsArray()?.FirstOrDefault();
            int rows = gridNode?["Rows"]?.GetValue<int>() ?? 0;
            int columns = gridNode?["Columns"]?.GetValue<int>() ?? 0;

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
            Dictionary<string, double> cardMultiplierSpawner = new();

            if (cardMultiplierSpawnerNode != null)
            {
                foreach (var entry in cardMultiplierSpawnerNode)
                {
                    int multiplier = entry!["Card Multiplier"]?.GetValue<int>() ?? 0;
                    double probability = entry!["Probaility %"]?.GetValue<double>() ?? 0.0;

                    string key = $"Card Multiplier {multiplier}";
                    cardMultiplierSpawner[key] = probability;
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

            List<KeyValuePair<string, Dictionary<string, double>>> allSpawnerTable;

            return new GridBasedGameMechanic_Default(
                new SpawnSymbolsProvider_Default(columns, rows, [new("", bonusSpawnerProbabilities), new("Card Front", cardMultiplierSpawner)]),
                new LineBasedEvaluatorPayer_Default(paylines, paytable)
            );
*/