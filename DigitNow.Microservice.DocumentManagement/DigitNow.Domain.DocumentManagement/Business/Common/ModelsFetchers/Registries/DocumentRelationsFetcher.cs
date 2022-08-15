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

        public IReadOnlyList<DocumentDepartmentModel> DocumentDepartments 
            => GetItems<GenericDocumentsDepartmentsFetcher, DocumentDepartmentModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentCategories 
            => GetItems<GenericDocumentsCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories 
            => GetItems<GenericDocumentsInternalCategoriesFetcher, DocumentCategoryModel>();

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDocumentsCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDocumentsInternalCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDocumentsDepartmentsFetcher>();         
        }

        public DocumentRelationsFetcher UseDocumentsContext(DocumentsFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<DocumentsUsersFetcher>(context);

            return this;
        }
    }
}
