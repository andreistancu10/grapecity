using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class VirtualDocumentAggregate<T>
        where T : VirtualDocument
    {
        public T VirtualDocument { get; set; }
        internal UserModel CurrentUser { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
        internal IReadOnlyList<DepartmentModel> Departments { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> InternalCategories { get; set; }
        internal IReadOnlyList<DocumentStatusTranslationModel> DocumentStatusTranslations { get; set; }
        internal IReadOnlyList<DocumentTypeTranslationModel> DocumentTypeTranslations { get; set; }
    }
}