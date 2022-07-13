using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    public static class PredicateFactory
    {
        public static IList<Expression<Func<T, bool>>> CreatePredicatesList<T>(params Expression<Func<T, bool>>[] predicates)
            where T : class => new List<Expression<Func<T, bool>>>(predicates);
    }
}
