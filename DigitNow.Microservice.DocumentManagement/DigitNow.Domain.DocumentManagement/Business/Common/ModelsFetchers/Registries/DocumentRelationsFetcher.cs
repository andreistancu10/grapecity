using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> DocumentUsers 
            => GetItems<DocumentsUsersFetcher, UserModel>();

        public IReadOnlyList<DepartmentModel> DocumentDepartments 
            => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentCategories 
            => GetItems<GenericDocumentsCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories 
            => GetItems<GenericDocumentsInternalCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<DocumentStatusTranslationModel> DocumentStatusTranslations
            => GetItems<DocumentStatusTranslationsFetcher, DocumentStatusTranslationModel>();

        public IReadOnlyList<DocumentTypeTranslationModel> DocumentTypesTranslations
            => GetItems<DocumentTypeTranslationsFetcher, DocumentTypeTranslationModel>();

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDocumentsCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDocumentsInternalCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>();                
        }

        public DocumentRelationsFetcher UseDocumentsContext(DocumentsFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<DocumentsUsersFetcher>(context);

            return this;
        }

        public DocumentRelationsFetcher UseTranslationsContext(LanguageFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<DocumentStatusTranslationsFetcher>(context)
                .UseRemoteFetcher<DocumentTypeTranslationsFetcher>(context);

            return this;
        }
    }
}
