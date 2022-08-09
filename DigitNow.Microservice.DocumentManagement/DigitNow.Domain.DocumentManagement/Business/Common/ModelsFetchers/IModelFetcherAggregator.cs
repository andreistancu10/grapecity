namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcherAggregator
    {
        IModelFetcher GetFetcher<TFetcher>()
            where TFetcher : IModelFetcher;

        IModelFetcherAggregator UseGenericRemoteFetcher<TFetcher>()
            where TFetcher : IModelFetcher;

        IModelFetcherAggregator UseRemoteFetcher<TFetcher>(IModelFetcherContext context)
            where TFetcher: IModelFetcher;

        IModelFetcherAggregator UseGenericInternalFetcher<TFetcher>()
            where TFetcher : IModelFetcher;

        IModelFetcherAggregator UseInternalFetcher<TFetcher>(IModelFetcherContext context)
            where TFetcher : IModelFetcher;

        Task<IModelFetcherAggregator> TriggerFetchersAsync(CancellationToken token);
    }

    internal class ModelFetcherAggregator : IModelFetcherAggregator
    {
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<Type, ModelFetcherRegistryEntry> RemoteFetchers = new();
        private Dictionary<Type, ModelFetcherRegistryEntry> InternalFetchers = new();

        public ModelFetcherAggregator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IModelFetcher GetFetcher<TFetcher>() where TFetcher : IModelFetcher
        {
            var modelFetcherType = typeof(TFetcher);
            if (RemoteFetchers.ContainsKey(modelFetcherType))
            {
                return RemoteFetchers[modelFetcherType].ModelFetcher;
            }
            else if (InternalFetchers.ContainsKey(modelFetcherType))
            {
                return InternalFetchers[modelFetcherType].ModelFetcher;
            }
            return null;
        }

        public IModelFetcherAggregator UseGenericRemoteFetcher<TFetcher>()
            where TFetcher : IModelFetcher
            => UseRemoteFetcher<TFetcher>(new ModelFetcherContext());

        public IModelFetcherAggregator UseRemoteFetcher<TFetcher>(IModelFetcherContext fetcherContext)
            where TFetcher : IModelFetcher
        {
            var modelFetcher = Activator.CreateInstance(typeof(TFetcher), _serviceProvider) as IModelFetcher;
            RemoteFetchers[typeof(TFetcher)] = new ModelFetcherRegistryEntry
            {
                ModelFetcher = modelFetcher,
                ModelFetcherContext = fetcherContext
            };
            return this;
        }

        public IModelFetcherAggregator UseRemoteFetcher(IModelFetcher modelFetcher, IModelFetcherContext fetcherContext)
        {
            RemoteFetchers[modelFetcher.GetType()] = new ModelFetcherRegistryEntry
            {
                ModelFetcher = modelFetcher,
                ModelFetcherContext = fetcherContext
            };
            return this;
        }

        public IModelFetcherAggregator UseGenericInternalFetcher<TFetcher>()
            where TFetcher : IModelFetcher
            => UseInternalFetcher<TFetcher>(new ModelFetcherContext());

        public IModelFetcherAggregator UseInternalFetcher<TFetcher>(IModelFetcherContext fetcherContext)
            where TFetcher : IModelFetcher
        {
            var modelFetcher = Activator.CreateInstance(typeof(TFetcher), _serviceProvider) as IModelFetcher;
            InternalFetchers[typeof(TFetcher)] = new ModelFetcherRegistryEntry
            {
                ModelFetcher = modelFetcher,
                ModelFetcherContext = fetcherContext
            };
            return this;
        }

        public IModelFetcherAggregator UseInternalFetcher(IModelFetcher modelFetcher, IModelFetcherContext fetcherContext)
        {
            InternalFetchers[modelFetcher.GetType()] = new ModelFetcherRegistryEntry
            {
                ModelFetcher = modelFetcher,
                ModelFetcherContext = fetcherContext
            };
            return this;
        }

        public virtual async Task<IModelFetcherAggregator> TriggerFetchersAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(
                TriggerRemoteFetchersAsync(cancellationToken),
                TriggerInternalFetchersAsync(cancellationToken)
            );
            return this;
        }

        protected virtual async Task TriggerRemoteFetchersAsync(CancellationToken cancellationToken)
        {
            var externalFetchTasks = new List<Task>();
            foreach (var entryPair in RemoteFetchers)
            {
                var registryEntiry = entryPair.Value;

                externalFetchTasks.Add(registryEntiry.ModelFetcher.FetchAsync(registryEntiry.ModelFetcherContext, cancellationToken));
            }
            await Task.WhenAll(externalFetchTasks);
        }

        protected virtual async Task TriggerInternalFetchersAsync(CancellationToken cancellationToken)
        {
            foreach (var entryPair in InternalFetchers)
            {
                var registryEntry = entryPair.Value;

                await registryEntry.ModelFetcher.FetchAsync(registryEntry.ModelFetcherContext, cancellationToken);
            }
        }

        private class ModelFetcherRegistryEntry
        {
            public IModelFetcher ModelFetcher { get; set; }
            public IModelFetcherContext ModelFetcherContext { get; set; }
        }
    }
}
