namespace SlotLab.Engine.Models
{ 
   /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public class GridPayoutCalculator 
    {
        public GridEvaluatorOutputData gridEvaluatorOutputData;

        public GridPayoutCalculator(GridEvaluatorOutputData gridEvaluatorOutputData)
        {
            this.gridEvaluatorOutputData = gridEvaluatorOutputData;
        }
    }
}