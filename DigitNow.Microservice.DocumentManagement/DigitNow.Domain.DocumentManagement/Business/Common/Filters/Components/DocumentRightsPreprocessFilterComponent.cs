using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components
{
    internal class DocumentRightsPreprocessFilterComponent : DataExpressionFilterComponent<Document, DocumentRightsPreprocessFilterComponentContext>
    {
        private readonly DocumentDepartmentRightsPreprocessFilterBuilder _departmentRightsFilterBuilder;
        private readonly DocumentUserRightsPreprocessFilterBuilder _userRightsFilterBuilder;

        public DocumentRightsPreprocessFilterComponent(
            IServiceProvider serviceProvider,
            DocumentDepartmentRightsFilter departmentRightsFilter,
            DocumentUserRightsFilter userRightsFilter)
            : base(serviceProvider)
        {
            _departmentRightsFilterBuilder = new DocumentDepartmentRightsPreprocessFilterBuilder(serviceProvider, departmentRightsFilter);
            _userRightsFilterBuilder = new DocumentUserRightsPreprocessFilterBuilder(serviceProvider, userRightsFilter);
        }

        public override DataExpressions<Document> ExtractDataExpressions(DocumentRightsPreprocessFilterComponentContext context)
        {
            if (context.UserDepartmentIds.Contains(default))
            {
                return _departmentRightsFilterBuilder.Build();
            }
            return _userRightsFilterBuilder.Build();
        }
    }
}
