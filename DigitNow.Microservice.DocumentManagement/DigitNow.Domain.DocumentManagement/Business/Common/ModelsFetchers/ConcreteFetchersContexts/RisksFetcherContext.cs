using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class RisksFetcherContext : ModelFetcherContext
    {
        public IList<Risk> Risks
        {
            get => this[nameof(Risks)] as IList<Risk>;
            set => this[nameof(Risks)] = value;
        }
    }
}
