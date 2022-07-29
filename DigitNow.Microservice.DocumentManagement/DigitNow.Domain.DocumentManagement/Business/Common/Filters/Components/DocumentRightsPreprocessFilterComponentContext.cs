using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components
{
    internal class DocumentRightsPreprocessFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public List<long> UserDepartmentIds { get; set; }
    }
}
