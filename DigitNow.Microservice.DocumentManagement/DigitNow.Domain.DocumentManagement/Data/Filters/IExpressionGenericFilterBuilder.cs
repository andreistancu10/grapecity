﻿using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal interface IExpressionGenericFilterBuilder<T>
        where T : IExtendedEntity
    {
        IList<Expression<Func<T, bool>>> Build();
    }
}
