namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public abstract class GameMechanicInputData
    {
        public double bet;
        public GameMechanicInputData (double bet)
        {
            this.bet = bet;
        }
    }
}