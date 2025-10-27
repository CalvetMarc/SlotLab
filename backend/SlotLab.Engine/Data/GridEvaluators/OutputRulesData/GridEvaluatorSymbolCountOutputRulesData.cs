namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public class GridEvaluatorSymbolCountOutputRulesData : GridEvaluatorOutputRulesData
    {
        public Dictionary<string, List<(int row, int col)>> detectionsInfo;      
        public GridEvaluatorSymbolCountOutputRulesData(Dictionary<string, List<(int row, int col)>> detectionsInfo)
        {
            this.detectionsInfo = detectionsInfo;
        }
    }
}