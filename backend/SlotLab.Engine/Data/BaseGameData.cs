using SlotLab.Engine.Core;

namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a spin operation — visible symbols + optional metadata.
    /// </summary>
    public class BaseGameData
    {
        public decimal ActiveBet { get; set; }
        public bool IsAuto { get; set; }
        public IGameMechanicComponents GameMechanicComponents { get; init; } 
        public Rng NumbersGenerator { get; init; } 

        public BaseGameData(IGameMechanicComponents gameMechanicComponents, Rng numbersGenerator)
        {
            this.GameMechanicComponents = gameMechanicComponents;
            this.NumbersGenerator = numbersGenerator;

            IsAuto = false;
            ActiveBet = 1m;
        }

    }
}