using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class DocumentFilesFetcherContext : ModelFetcherContext
    {
        public IList<DocumentFileModel> DocumentFileModels
        {
            get => this[nameof(DocumentFileModels)] as IList<DocumentFileModel>;
            set => this[nameof(DocumentFileModels)] = value;
        }

        public DocumentFilesFetcherContext(IList<DocumentFileModel> documentFileModels)
        {
            DocumentFileModels = documentFileModels;
        }
    }
}