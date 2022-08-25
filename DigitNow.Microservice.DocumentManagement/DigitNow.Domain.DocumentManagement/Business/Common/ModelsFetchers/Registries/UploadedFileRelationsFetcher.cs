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
            Aggregator
                .UseRemoteFetcher<UploadedFilesUsersFetcher>(context)
                .UseInternalFetcher<DocumentFileMappingsFetcher>(context);

            return this;
        }
    }

    internal sealed class UploadedFileRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> UploadedFileUsers
            => GetItems<UploadedFilesUsersFetcher, UserModel>();

        public UploadedFileRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public UploadedFileRelationsFetcher UseUploadedFilesContext(UploadedFilesFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<UploadedFilesUsersFetcher>(context);

            return this;
        }
    }
}