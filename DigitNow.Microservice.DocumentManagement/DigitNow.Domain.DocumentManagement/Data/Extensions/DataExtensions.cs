using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.Extensions
{
    public static class DataExtensions
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }
            return queryable;
        }
    }
}