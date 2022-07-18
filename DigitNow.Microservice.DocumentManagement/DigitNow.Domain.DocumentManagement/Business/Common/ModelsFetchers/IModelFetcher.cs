using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcher
    {
        bool IsInternal { get; }

        Task<IReadOnlyList<object>> FetchAsync(object context, CancellationToken cancellationToken);
    }

    internal interface IModelFetcher<T, TContext> : IModelFetcher
        where T : class
        where TContext : IModelFetcherContext
    {
        Task<IReadOnlyList<T>> FetchAsync(TContext context, CancellationToken cancellationToken);
    }

    internal abstract class ModelFetcher<T, TContext> : IModelFetcher<T, TContext>
        where T : class
        where TContext : IModelFetcherContext
    {
        public abstract bool IsInternal { get; }

        public abstract Task<IReadOnlyList<T>> FetchAsync(TContext context, CancellationToken cancellationToken);

        public async Task<IReadOnlyList<object>> FetchAsync(object context, CancellationToken cancellationToken)
        {
            return await FetchAsync((TContext)context, cancellationToken);
        }
    }
}
