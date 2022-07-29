using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters
{
    internal abstract class DataExpressionFilterComponent<T, TContext> : IDataExpressionFilterComponent<T, TContext>
        where T : IExtendedEntity
        where TContext : IDataExpressionFilterComponentContext
    {
        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }

        public DataExpressionFilterComponent(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract DataExpressions<T> ExtractDataExpressions(TContext context);
    }
}
