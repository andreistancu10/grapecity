using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions
{
    internal interface IDataExpressionFilterComponent<T, TContext>
        where T : IExtendedEntity
        where TContext : IDataExpressionFilterComponentContext
    {
        public abstract DataExpressions<T> ExtractDataExpressions(TContext context);

        public IList<Expression<Func<T, bool>>> ExtractPredicates(TContext context) =>
            ExtractDataExpressions(context).ToPredicates();
    }
}
