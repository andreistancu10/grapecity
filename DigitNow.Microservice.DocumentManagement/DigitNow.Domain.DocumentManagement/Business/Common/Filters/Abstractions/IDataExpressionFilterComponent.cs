using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions
{
    internal interface IDataExpressionFilterComponent<T, TContext>
        where T : IExtendedEntity
        where TContext : IDataExpressionFilterComponentContext
    {
        Task<DataExpressions<T>> ExtractDataExpressionsAsync(TContext context, CancellationToken token);

        Task<IList<Expression<Func<T, bool>>>> ExtractPredicatesAsync(TContext context, CancellationToken token);
    }
}
