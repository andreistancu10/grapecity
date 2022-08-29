using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class GeneralObjectivesFetcherContext: ModelFetcherContext
    {
        public IList<GeneralObjective> GeneralObjectives
        {
            get => this[nameof(GeneralObjective)] as IList<GeneralObjective>;
            set => this[nameof(GeneralObjective)] = value;
        }
    }
}
