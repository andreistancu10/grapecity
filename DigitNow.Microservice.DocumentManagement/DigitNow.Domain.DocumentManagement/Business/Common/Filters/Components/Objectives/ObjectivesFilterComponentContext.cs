using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class ObjectivesFilterComponentContext: DataExpressionFilterComponentContext
    {
        public ObjectiveFilter ObjectiveFilter { get; set; }
    }
}
