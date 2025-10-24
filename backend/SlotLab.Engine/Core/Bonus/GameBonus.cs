using SlotLab.Engine.Models;

namespace SlotLab.Engine.Core.Base
{
    public class GameBonus
    {
        protected double totalWin;
        protected bool lastBonusActive;

        protected readonly IBonusTrigger bonusTrigger;
        protected readonly IBonusStateHandler bonusStateHandler;
        public event Action<Dictionary<string, object>>? OnBonusStart;
        public event Action<double>? OnBonusEnd;

        public GameBonus(IBonusTrigger trigger, IBonusStateHandler bonusStateHandler)
        {
            this.bonusTrigger = trigger;
            this.bonusStateHandler = bonusStateHandler;
        }

        public (bool, Dictionary<string, object>?) CanBonusStart(Dictionary<string, object> gameData) => bonusTrigger.CheckBonusActivation(gameData);

        public void StartBonus(Dictionary<string, object>? metadata) => bonusStateHandler.Enter(metadata);

        public void TickBonus(Dictionary<string, object>? metadata) => bonusStateHandler.Update(metadata);
        public bool HasBonusFinished(Dictionary<string, object>? metadata) => bonusStateHandler.HasBonusFinished();
    }

}
