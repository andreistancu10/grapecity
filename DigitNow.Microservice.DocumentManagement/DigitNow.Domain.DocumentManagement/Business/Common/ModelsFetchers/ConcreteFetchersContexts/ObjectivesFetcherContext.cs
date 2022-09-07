using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class ObjectivesFetcherContext : ModelFetcherContext
    {
        public IList<SpecificObjective> Objectives
        {
            get => this[nameof(Objectives)] as IList<SpecificObjective>;
            set => this[nameof(Objectives)] = value;
        }
    }
}
