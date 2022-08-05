using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class SpecialRegisterRelationsFetcher : RelationalAggregateFetcher<SpecialRegisterFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IGenericCategoryModelFetcher<SpecialRegisterFetcherContext> _documentCategoriesFetcher;

        public SpecialRegisterRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReadOnlyList<DocumentCategoryModel> CategoryModels => GetItems(_documentCategoriesFetcher);

        protected override void AddRemoteFetchers()
        {
            _documentCategoriesFetcher = new GenericCategoryModelFetcher<SpecialRegisterFetcherContext>(_serviceProvider);
            RemoteFetchers.Add(_documentCategoriesFetcher);
        }
    }
}