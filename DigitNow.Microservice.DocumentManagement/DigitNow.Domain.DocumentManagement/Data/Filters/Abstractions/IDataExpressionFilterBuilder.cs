using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Abstractions
{
    internal interface IDataExpressionFilterBuilder<T, TFilter> : IDataExpressionGenericFilterBuilder<T>
        where T : IExtendedEntity
        where TFilter : class, new()
    { }
}
