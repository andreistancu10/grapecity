using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal interface IExpressionFilterBuilder<T, TFilter> : IExpressionGenericFilterBuilder<T>
        where T : IExtendedEntity
        where TFilter : class, new()
    { }

    internal abstract class ExpressionFilterBuilder<T, TFilter> : IExpressionFilterBuilder<T, TFilter>
        where T : IExtendedEntity
        where TFilter : class, new()
    {
        protected List<Expression<Func<T, bool>>> GeneratedFilters
        {
            get;
            private set;
        }

        protected TFilter EntityFilter
        {
            get;
            private set;
        }

        protected ExpressionFilterBuilder(TFilter entityFilter)
        {
            GeneratedFilters = new List<Expression<Func<T, bool>>>();
            EntityFilter = entityFilter;
        }

        protected abstract void InternalBuild();

        public IList<Expression<Func<T, bool>>> Build()
        {
            InternalBuild();
            return GeneratedFilters;
        }
    }
}
