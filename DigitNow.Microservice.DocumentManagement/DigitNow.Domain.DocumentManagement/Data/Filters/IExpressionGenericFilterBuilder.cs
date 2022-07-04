using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal interface IExpressionGenericFilterBuilder<T>
        where T : IExtendedEntity
    {
        IList<Expression<Func<T, bool>>> Build();
    }
}
