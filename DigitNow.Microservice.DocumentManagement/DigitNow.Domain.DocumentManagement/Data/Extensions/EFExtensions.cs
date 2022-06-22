using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Data.EFExtensions
{
    public static class EFExtensions
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> queriable, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            foreach (var include in includes)
            {
                queriable.Include(include);
            }

            return queriable;
        }
    }
}
