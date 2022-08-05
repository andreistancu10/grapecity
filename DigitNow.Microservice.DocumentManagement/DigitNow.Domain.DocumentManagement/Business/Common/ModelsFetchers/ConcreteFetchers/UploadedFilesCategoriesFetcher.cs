using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal sealed class UploadedFilesDocumentCategoriesFetcher : GenericCategoryModelFetcher<DocumentsFetcherContext>
    {
        public UploadedFilesDocumentCategoriesFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}