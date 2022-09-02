using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class SpecificObjectivesFilterComponenetContext : DataExpressionFilterComponentContext
    {
        public SpecificObjectiveFilter SpecificObjectiveFilter { get; set; }
    }
}
