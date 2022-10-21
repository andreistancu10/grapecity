using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class ProcedureHistoriesFetcherContext : ModelFetcherContext
    {
        public IList<ProcedureHistory> ProcedureHistories
        {
            get => this[nameof(ProcedureHistories)] as IList<ProcedureHistory>;
            set => this[nameof(ProcedureHistories)] = value;
        }
    }
}
