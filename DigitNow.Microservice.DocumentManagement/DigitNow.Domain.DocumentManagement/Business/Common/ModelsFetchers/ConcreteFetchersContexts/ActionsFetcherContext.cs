namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class ActionsFetcherContext : ModelFetcherContext
    {
        public IList<Data.Entities.Action> Actions
        {
            get => this[nameof(Actions)] as IList<Data.Entities.Action>;
            set => this[nameof(Actions)] = value;
        }
    }
}