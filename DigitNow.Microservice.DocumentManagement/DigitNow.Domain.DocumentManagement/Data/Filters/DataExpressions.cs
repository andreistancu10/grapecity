using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal class DataExpressions<T> : List<DataExpression<T>>
        where T : IExtendedEntity
    {
        public void Add(Expression<Func<T, bool>> expression)
        {
            this.Add(new DataExpression<T>(expression));
        }

        public IList<Expression<Func<T, bool>>> ToPredicates()
        {
            var result = new List<Expression<Func<T, bool>>>();
            foreach (var predicate in this)
            {
                result.Add(predicate.ToPredicate());
            }
            return result;
        }
    }
}
