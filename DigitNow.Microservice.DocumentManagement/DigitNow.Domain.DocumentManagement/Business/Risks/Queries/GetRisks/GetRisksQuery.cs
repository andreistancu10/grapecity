using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRisks
{
    public class GetRisksQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public RiskFilter Filter { get; set; }
    }
}
