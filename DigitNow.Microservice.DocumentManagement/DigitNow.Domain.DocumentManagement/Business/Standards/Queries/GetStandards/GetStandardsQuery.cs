using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetStandards
{
    public class GetStandardsQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public StandardFilter Filter { get; set; }
    }
}
