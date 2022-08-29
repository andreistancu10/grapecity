using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.Get
{
    public class GetSpecificObjectiveQuery : IQuery<GetSpecificObjectiveResponse>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public SpecificObjectiveFilter Filter { get; set; }
    }
}
