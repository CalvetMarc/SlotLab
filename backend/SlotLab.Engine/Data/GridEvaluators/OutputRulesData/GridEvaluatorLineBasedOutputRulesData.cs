namespace SlotLab.Engine.Models
{
    /// <summary>
    /// Represents the result of a complete play operation — spin + evaluation info
    /// </summary>
    public class GridEvaluatorLineBasedOutputRulesData : GridEvaluatorOutputRulesData
    {
        public List<(int lineNumber, string symbol, int lineSymbolsCount)> linesInfo;      
        public GridEvaluatorLineBasedOutputRulesData(List<(int lineNumber, string symbol, int lineSymbolsCount)> linesInfo)
        {
            this.linesInfo = linesInfo;
        }
    }
}