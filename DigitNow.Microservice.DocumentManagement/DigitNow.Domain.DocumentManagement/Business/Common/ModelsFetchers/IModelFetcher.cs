using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcher<T, TContext>
        where T : class
        where TContext : IModelFetcherContext
    {
        Task<IReadOnlyList<T>> FetchAsync(TContext context, CancellationToken cancellationToken);
    }

    internal abstract class ModelFetcher<T, TContext> : IModelFetcher<T, TContext>
        where T : class
        where TContext : IModelFetcherContext
    {
        public abstract Task<IReadOnlyList<T>> FetchAsync(TContext context, CancellationToken cancellationToken);
    }
}
