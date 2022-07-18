using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class RelationalAggregateFetcher<TContext>
        where TContext : IModelFetcherContext
    {
        protected List<IModelFetcher> Fetchers { get; }

        public virtual async Task FetchRelationshipsAsync(TContext context, CancellationToken cancellationToken)
        {
            var internalFetchTasks = new List<Task>();
            var externalFetchTasks = new List<Task>();
            foreach (var item in Fetchers)
            {
                if (item.IsInternal)
                {
                    internalFetchTasks.Add(item.FetchAsync(context, cancellationToken));
                }
                else
                {
                    externalFetchTasks.Add(item.FetchAsync(context, cancellationToken));
                }
            }

            await Task.WhenAll(externalFetchTasks);

            foreach (var internalFetchTask in internalFetchTasks)
            {
                await internalFetchTask;
            }
        }
    }
}
