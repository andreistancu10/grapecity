using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal abstract class BaseRelationsFetcher
    {
        protected IModelFetcherAggregator Aggregator
        {
            get;
            private set;
        }

        public BaseRelationsFetcher(IServiceProvider serviceProvider)
        {
            Aggregator = new ModelFetcherAggregator(serviceProvider);
        }

        public Task TriggerFetchersAsync(CancellationToken token) =>
            Aggregator.TriggerFetchersAsync(token);

        protected virtual List<TModel> GetItems<T, TModel>() where T : IModelFetcher
        {
            return Aggregator.GetFetcher<T>().Items
                .Cast<TModel>().ToList();
        }
    }
}
