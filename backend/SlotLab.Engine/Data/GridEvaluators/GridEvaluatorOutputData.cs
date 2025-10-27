namespace SlotLab.Engine.Models
{ 
   /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    /// 
    
    public abstract class GridEvaluatorOutputData 
    {
        public GridEvaluatorOutputRulesData evaluatorOutputRulesData;

        public GridEvaluatorOutputData(GridEvaluatorOutputRulesData evaluatorOutputRulesData)
        {
            this.evaluatorOutputRulesData = evaluatorOutputRulesData;
        }
    }
}