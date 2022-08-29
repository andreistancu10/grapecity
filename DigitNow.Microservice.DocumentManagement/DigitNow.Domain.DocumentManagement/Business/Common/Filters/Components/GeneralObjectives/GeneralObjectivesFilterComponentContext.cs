using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.GeneralObjectives
{
    internal class GeneralObjectivesFilterComponentContext : DataExpressionFilterComponentContext
    {
        public GeneralObjectiveFilter GeneralObjectiveFilter { get; set; }
    }
}
