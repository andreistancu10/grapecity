using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters
{
    internal abstract class DataExpressionFilterComponent<T, TContext> : IDataExpressionFilterComponent<T, TContext>
        where T : IExtendedEntity
        where TContext : IDataExpressionFilterComponentContext
    {
        #region [ Fields ]

        private DataExpressions<T> _dataExpressions = new DataExpressions<T>();

        #endregion

        #region [ Properties ]

        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }

        #endregion

        #region [ Construction ]

        public DataExpressionFilterComponent(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        #endregion

        #region [ DataExpressionFilterComponent ]

        protected virtual Task<DataExpressions<T>> SetBuiltinDataExpressionsAsync(TContext context, CancellationToken token) => null;

        protected abstract Task<DataExpressions<T>> SetCustomDataExpressionsAsync(TContext context, CancellationToken token);

        #endregion

        #region [ IDataExpressionFilterComponent ]

        public async Task<DataExpressions<T>> ExtractDataExpressionsAsync(TContext context, CancellationToken token)
        {
            var builtinExpressions = await SetBuiltinDataExpressionsAsync(context, token);
            if (builtinExpressions != null)
            {
                _dataExpressions.AddRange(builtinExpressions);
            }

            _dataExpressions.AddRange(await SetCustomDataExpressionsAsync(context, token));
            return _dataExpressions;
        }

        public async Task<IList<Expression<Func<T, bool>>>> ExtractPredicatesAsync(TContext context, CancellationToken token) =>
            (await ExtractDataExpressionsAsync(context, token)).ToPredicates();

        #endregion
    }
}
