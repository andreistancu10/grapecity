using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentFileRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<DocumentCategoryModel> UploadedFileCategoryModels
            => GetItems<GenericDocumentsCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<UserModel> UploadedFileUsers
            => GetItems<UploadedFilesUsersFetcher, UserModel>();

        public IReadOnlyList<DocumentFileMappingModel> DocumentFileMappingModels
            => GetItems<DocumentFileMappingsFetcher, DocumentFileMappingModel>();

        public DocumentFileRelationsFetcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDocumentsCategoriesFetcher>();
        }

        public DocumentFileRelationsFetcher UseDocumentFilesContext(DocumentFilesFetcherContext context)
        {
            return this;
        }

        public DocumentFileRelationsFetcher UseUploadedFilesContext(UploadedFilesFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<UploadedFilesUsersFetcher>(context)
                .UseInternalFetcher<DocumentFileMappingsFetcher>(context); ;

            return this;
        }
    }
}