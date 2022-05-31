using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Nest;

namespace ShiftIn.Domain.TenantNotification.utils
{
    public sealed class FilterDescriptor<T> : List<Func<QueryContainerDescriptor<T>, QueryContainer>>
        where T : class
    {
        private const string _stringContainsTemplate = "*{0}*";
        private const string _stringEndWithTemplate = "*{0}";

        public FilterDescriptor<T> Query(Expression<Func<T, string>> selector,
            string parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(!string.IsNullOrEmpty(parameter), selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, string>> selector,
            string parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Contain => item =>
                        item.Wildcard(m => m.Field(selector).Value(FormatStringLike(parameter))),
                    FilterTypeCode.Equal => item => item.Match(m => m.Field(selector).Query(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Match(m => m.Field(selector).Query(parameter)),
                    FilterTypeCode.StartWith => item => item.Prefix(p => p.Field(selector).Value(parameter)),
                    FilterTypeCode.EndWith => item =>
                        item.Wildcard(m => m.Field(selector).Value(FormatStringEndWith(parameter))),
                    _ => item => item.Wildcard(m => m.Field(selector).Value(parameter))
                });

            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, int?>> selector,
            int? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, int?>> selector,
            int? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.GreaterThan => item => item.LongRange(m => m.Field(selector).GreaterThan(parameter)),
                    FilterTypeCode.GreaterThanOrEqual => item =>
                        item.LongRange(m => m.Field(selector).GreaterThanOrEquals(parameter)),
                    FilterTypeCode.LessThan => item => item.LongRange(m => m.Field(selector).LessThan(parameter)),
                    FilterTypeCode.LessThanOrEqual => item =>
                        item.LongRange(m => m.Field(selector).LessThanOrEquals(parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, long?>> selector,
            long? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, long?>> selector,
            long? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.GreaterThan => item => item.LongRange(m => m.Field(selector).GreaterThan(parameter)),
                    FilterTypeCode.GreaterThanOrEqual => item =>
                        item.LongRange(m => m.Field(selector).GreaterThanOrEquals(parameter)),
                    FilterTypeCode.LessThan => item => item.LongRange(m => m.Field(selector).LessThan(parameter)),
                    FilterTypeCode.LessThanOrEqual => item =>
                        item.LongRange(m => m.Field(selector).LessThanOrEquals(parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, decimal?>> selector,
            decimal? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, decimal?>> selector,
            decimal? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
            {
                double _parameter = (double) parameter;
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.GreaterThan => item => item.Range(m => m.Field(selector).GreaterThan(_parameter)),
                    FilterTypeCode.GreaterThanOrEqual => item =>
                        item.Range(m => m.Field(selector).GreaterThanOrEquals(_parameter)),
                    FilterTypeCode.LessThan => item => item.Range(m => m.Field(selector).LessThan(_parameter)),
                    FilterTypeCode.LessThanOrEqual => item =>
                        item.Range(m => m.Field(selector).LessThanOrEquals(_parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            }

            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, double?>> selector,
            double? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, double?>> selector,
            double? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.GreaterThan => item => item.Range(m => m.Field(selector).GreaterThan(parameter)),
                    FilterTypeCode.GreaterThanOrEqual => item =>
                        item.Range(m => m.Field(selector).GreaterThanOrEquals(parameter)),
                    FilterTypeCode.LessThan => item => item.Range(m => m.Field(selector).LessThan(parameter)),
                    FilterTypeCode.LessThanOrEqual => item =>
                        item.Range(m => m.Field(selector).LessThanOrEquals(parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, DateTime?>> selector,
            DateTime? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, DateTime?>> selector,
            DateTime? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.GreaterThan => item => item.DateRange(m => m.Field(selector).GreaterThan(parameter)),
                    FilterTypeCode.GreaterThanOrEqual => item =>
                        item.DateRange(m => m.Field(selector).GreaterThanOrEquals(parameter)),
                    FilterTypeCode.LessThan => item => item.DateRange(m => m.Field(selector).LessThan(parameter)),
                    FilterTypeCode.LessThanOrEqual => item =>
                        item.DateRange(m => m.Field(selector).LessThanOrEquals(parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, bool?>> selector,
            bool? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.HasValue, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, bool?>> selector,
            bool? parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Term(m => m.Field(selector).Value(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Term(m => m.Field(selector).Value(parameter)),
                    _ => item => item.Term(m => m.Field(selector).Value(parameter))
                });
            return this;
        }

        public FilterDescriptor<T> Query(Expression<Func<T, long?>> selector,
            List<long> parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            return Query(parameter.Count > 0, selector, parameter, filterModePredicate);
        }

        public FilterDescriptor<T> Query(bool condition,
            Expression<Func<T, long?>> selector,
            List<long> parameter,
            Func<FilterTypeCode> filterModePredicate)
        {
            if (condition)
                Add(filterModePredicate() switch
                {
                    FilterTypeCode.Equal => item => item.Terms(m => m.Field(selector).Terms(parameter)),
                    FilterTypeCode.NotEqual => item => !item.Terms(m => m.Field(selector).Terms(parameter)),
                    _ => item => item.Terms(m => m.Field(selector).Terms(parameter))
                });
            return this;
        }

        private string FormatStringLike(string value)
        {
            string like = string.Format(_stringContainsTemplate, value);

            return like;
        }

        private string FormatStringEndWith(string value)
        {
            return string.Format(_stringEndWithTemplate, value);
        }
    }
}