

namespace SlotLab.Engine.Core
{
    public class FreeSpinsBonusStateHandler_Default : IBonusStateHandler
    {
        private int currentSpins;

        public FreeSpinsBonusStateHandler_Default()
        {
            currentSpins = 0;
        }

        public void Enter(Dictionary<string, object>? metadata) //Sera un pair {"Spins", int num spins}
        {            
            if (metadata != null && metadata.TryGetValue("Spins", out var spinsValue))
            {
                // Intentem convertir el valor a int
                if (spinsValue is int spins)
                    currentSpins = spins;
                else
                    currentSpins = Convert.ToInt32(spinsValue);
            }
            else
            {
                currentSpins = 0;
            }
        }

        public void Update(Dictionary<string, object>? metadata)
        {
            currentSpins--;
        }

        public bool HasBonusFinished() => currentSpins <= 0;
    }
}