using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface ICachedModelFetcher<T, TContext> : IModelFetcher<T, TContext>
        where T : class
        where TContext : IModelFetcherContext
    {
        Task<IReadOnlyList<T>> FetchOrGetAsync(TContext context, CancellationToken cancellationToken);
    }

    internal abstract class CachedModelFetcher<T, TContext> : ICachedModelFetcher<T, TContext>
        where T : class
        where TContext : ModelFetcherContext
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private IReadOnlyList<T> _fetchedItems;

        public abstract Task<IReadOnlyList<T>> FetchAsync(TContext context, CancellationToken cancellationToken);

        public async Task<IReadOnlyList<T>> FetchOrGetAsync(TContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (_fetchedItems == null)
                {
                    _fetchedItems = await FetchAsync(context, cancellationToken);
                }
                return _fetchedItems;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
