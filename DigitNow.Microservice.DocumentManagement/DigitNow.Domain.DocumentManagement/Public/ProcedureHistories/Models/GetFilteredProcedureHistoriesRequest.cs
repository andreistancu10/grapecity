 namespace DigitNow.Domain.DocumentManagement.Public.ProcedureHistories.Models
{
    public class GetFilteredProcedureHistoriesRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ProcedureHistoriesFilterDto Filter { get; set; }
    }
}
