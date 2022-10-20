using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetFilteredProcedures
{
    public class GetFilteredProcedureHistoriesQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ProcedureHistoryFilter Filter { get; set; }
    }
}
