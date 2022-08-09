using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcher
    {
        Type ModelType { get; }

        IReadOnlyList<object> Items { get; }

        Task FetchAsync(object context, CancellationToken cancellationToken);
    }

    internal interface IModelFetcher<out T, TContext> : IModelFetcher
        where T : class
        where TContext: IModelFetcherContext
    {
        IReadOnlyList<T> Models { get; }

        Task FetchAsync(TContext context, CancellationToken cancellationToken);
    }

    internal abstract class ModelFetcher<T, TContext> : IModelFetcher<T, TContext>
        where T : class
        where TContext : IModelFetcherContext
    {
        private List<T> _models = new List<T>();

        public Type ModelType => typeof(T);

        public IReadOnlyList<T> Models => _models;

        IReadOnlyList<object> IModelFetcher.Items => _models;

        protected abstract Task<List<T>> FetchInternalAsync(TContext context, CancellationToken cancellationToken);

        public async Task FetchAsync(TContext context, CancellationToken cancellationToken)
        {
            _models = await FetchInternalAsync(context, cancellationToken);
        }

        async Task IModelFetcher.FetchAsync(object context, CancellationToken cancellationToken)
        {
            await FetchAsync((TContext)context, cancellationToken);
        }
    }
}
