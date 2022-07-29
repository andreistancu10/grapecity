using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal abstract class DataExpressionFilterBuilder<T, TFilter> : IDataExpressionFilterBuilder<T, TFilter>
        where T : IExtendedEntity
        where TFilter : class, new()
    {
        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }

        protected TFilter EntityFilter
        {
            get;
            private set;
        }

        protected DataExpressions<T> EntityPredicates
        {
            get;
            private set;
        }

        protected DataExpressionFilterBuilder(IServiceProvider serviceProvider, TFilter entityFilter)
        {
            ServiceProvider = serviceProvider;
            EntityFilter = entityFilter;
            EntityPredicates = new DataExpressions<T>();            
        }

        protected abstract void InternalBuild();

        public DataExpressions<T> Build()
        {
            InternalBuild();
            return EntityPredicates;
        }
    }
}
