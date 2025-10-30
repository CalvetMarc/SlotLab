using SlotLab.Engine.Core;
using SlotLab.Engine.Core.Base;

namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a spin operation — visible symbols + optional metadata.
    /// </summary>
    public class BaseGameData<TOutput> where TOutput : GridEvaluatorOutputRulesData
    {
        public decimal ActiveBet { get; set; }
        public bool IsAuto { get; set; }
        public GameEnvironmentMode GameEnvMode { get; init; }
        public GridBasedGameMechanic_Default<TOutput> GridGameMechanicComponents { get; init; }
        public Rng NumbersGenerator { get; init; } 

        public BaseGameData(GridBasedGameMechanic_Default<TOutput> gridGameMechanicComponents, Rng numbersGenerator, GameEnvironmentMode gameEnvMode)
        {
            this.GridGameMechanicComponents = gridGameMechanicComponents;
            this.NumbersGenerator = numbersGenerator;
            this.GameEnvMode = gameEnvMode;

            IsAuto = false;
            ActiveBet = 1m;
        }

    }
}