using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Risks
{
    internal class RisksFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public RiskFilter RiskFilter { get; set; }
    }
}
