using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Standards
{
    internal class StandardsFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public StandardFilter StandardFilter { get; set; }
    }
}
