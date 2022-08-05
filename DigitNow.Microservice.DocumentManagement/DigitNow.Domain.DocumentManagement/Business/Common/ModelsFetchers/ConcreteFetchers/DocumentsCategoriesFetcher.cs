using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal sealed class DocumentsCategoriesFetcher : GenericCategoryModelFetcher<DocumentsFetcherContext>
    {
        public DocumentsCategoriesFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}