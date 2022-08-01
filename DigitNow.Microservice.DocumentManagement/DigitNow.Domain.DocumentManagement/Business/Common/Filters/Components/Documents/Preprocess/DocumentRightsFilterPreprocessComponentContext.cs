using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess
{
    internal class DocumentRightsFilterPreprocessComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
        public DocumentDepartmentRightsFilter DepartmentRightsFilter { get; set; }
        public DocumentUserRightsFilter UserRightsFilter { get; set; }
    }
}
