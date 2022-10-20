using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class ProcedureHistoryRelationsFetcher: BaseRelationsFetcher
    {
        public ProcedureHistoryRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public IReadOnlyList<UserModel> Users => GetItems<ProcedureHistoriesUsersFetcher, UserModel>();

        public ProcedureHistoryRelationsFetcher UseProcedureHistoryFetcherContext(ProcedureHistoriesFetcherContext context)
        {
            Aggregator
                .UseInternalFetcher<ProcedureHistoriesUsersFetcher>(context);

            return this;
        }
    }
}
