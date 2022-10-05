using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class ProceduresFetcherContext: ModelFetcherContext
    {
        public IList<Procedure> Procedures
        {
            get => this[nameof(Procedures)] as IList<Procedure>;
            set => this[nameof(Procedures)] = value;
        }
    }
}
