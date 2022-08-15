using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal class DataExpression<T> : List<Expression<Func<T, bool>>>
        where T : IExtendedEntity
    {
        public DataExpression(Expression<Func<T, bool>> expression)
        {
            Add(expression);
        }

        public Expression<Func<T, bool>> ToPredicate() => this.FirstOrDefault();
    }
}
