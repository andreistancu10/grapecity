﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using HTSS.Platform.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.Extensions
{
    public static class DataExtensions
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> queryable, IList<Expression<Func<T, object>>> includes)
            where T : class => Includes(queryable, includes.ToArray());

        public static IQueryable<T> Includes<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }
            return queryable;
        }

        public static IQueryable<T> WhereAll<T>(this IQueryable<T> queryable, IList<Expression<Func<T, bool>>> predicates = null)
            where T : class
        {
            if (predicates == null) return queryable;
            foreach (var predicate in predicates)
            {
                queryable = queryable.Where(predicate);
            }
            return queryable;
        }

        internal static IQueryable<T> WhereMyFilter<T>(this IQueryable<T> queryable, DataExpression<T> dataExpression = null)
            where T: IExtendedEntity
        {
            if (dataExpression == null) return queryable;
            return queryable.Where(dataExpression.ToPredicate());
        }
    }
}