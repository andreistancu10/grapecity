using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Procedures
{
    public class ProcedureHistoryFilter : DataFilter
    {
        public ProcedureHistoryProceduresFilter ProcedureHistoryProceduresFilter { get; set; }
        public static ProcedureHistoryFilter Empty => new();
    }

    public class ProcedureHistoryProceduresFilter
    {
        public IList<long> ProcedureIds { get; set; }
    }
}
