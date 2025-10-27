namespace SlotLab.Engine.Models
{ 
    /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public abstract class GridEvaluatorInputData 
    {
        public List<List<string>> visibleGrid;
        public GridEvaluatorInputRulesData evaluatorInputRulesData;

        public GridEvaluatorInputData(List<List<string>> visibleGrid, GridEvaluatorInputRulesData evaluatorInputRulesData)
        {
            this.evaluatorInputRulesData = evaluatorInputRulesData;
            this.visibleGrid = visibleGrid;
        }
    }
}