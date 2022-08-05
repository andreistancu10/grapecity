namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal abstract class RelationalAggregateFetcher<TContext>
        where TContext : IModelFetcherContext
    {
        protected List<IModelFetcher> RemoteFetchers = new();
        protected List<IModelFetcher> InternalFetchers = new();

        private void InitializeFetchers()
        {
            RemoteFetchers.Clear();
            AddRemoteFetchers();

            InternalFetchers.Clear();            
            AddInternalFetchers();
        }

        protected virtual void AddRemoteFetchers() { }
        protected virtual void AddInternalFetchers() { }

        public virtual async Task TriggerFetchersAsync(TContext context, CancellationToken cancellationToken)
        {
            InitializeFetchers();

            await Task.WhenAll(
                TriggerRemoteFetchersAsync(context, cancellationToken),
                TriggerInternalFetchersAsync(context, cancellationToken)
            );
        }

        protected virtual async Task TriggerRemoteFetchersAsync(TContext context, CancellationToken cancellationToken)
        {
            var externalFetchTasks = new List<Task>();
            foreach (var remoteFetcher in RemoteFetchers)
            {
                externalFetchTasks.Add(remoteFetcher.FetchAsync(context, cancellationToken));
            }
            await Task.WhenAll(externalFetchTasks);
        }

        protected virtual async Task TriggerInternalFetchersAsync(TContext context, CancellationToken cancellationToken)
        {
            foreach (var internalFetcher in InternalFetchers)
            {
                await internalFetcher.FetchAsync(context, cancellationToken);
            }
        }

        protected virtual List<T> GetItems<T>(IModelFetcher<T, TContext> modelFetcher)
            where T : class => modelFetcher.Items.Cast<T>().ToList();
    }
}
