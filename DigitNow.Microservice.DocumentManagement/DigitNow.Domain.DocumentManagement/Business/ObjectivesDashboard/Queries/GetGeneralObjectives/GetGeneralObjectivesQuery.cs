using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives
{
    public class GetGeneralObjectivesQuery: IQuery<GetGeneralObjectivesResponse>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public GeneralObjectiveFilter Filter { get; set; }

    }
}
