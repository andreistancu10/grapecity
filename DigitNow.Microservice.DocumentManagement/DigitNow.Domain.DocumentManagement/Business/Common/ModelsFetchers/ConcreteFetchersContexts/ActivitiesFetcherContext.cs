using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class ActivitiesFetcherContext : ModelFetcherContext
    {
        public IList<Activity> Activities
        {
            get => this[nameof(Activities)] as IList<Activity>;
            set => this[nameof(Activities)] = value;
        }
    }
}