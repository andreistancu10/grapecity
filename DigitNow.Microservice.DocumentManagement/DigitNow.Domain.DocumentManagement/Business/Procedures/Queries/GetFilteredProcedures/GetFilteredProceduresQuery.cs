using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetFilteredProcedures
{
    public class GetFilteredProceduresQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ProcedureFilter Filter { get; set; }
    }
}
