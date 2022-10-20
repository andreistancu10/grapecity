namespace DigitNow.Domain.DocumentManagement.Public.ProcedureHistories.Models
{
    public class ProcedureHistoriesFilterDto
    {
        public ProcedureHistoryProceduresFilterDto ProceduresFilter { get; set; }
    }

    public class ProcedureHistoryProceduresFilterDto
    {
        public IList<long> ProcedureIds { get; set; }
    }
}