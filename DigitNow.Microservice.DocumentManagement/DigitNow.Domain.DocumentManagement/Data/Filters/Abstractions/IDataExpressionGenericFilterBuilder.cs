using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Abstractions
{
    internal interface IDataExpressionGenericFilterBuilder<T>
        where T : IExtendedEntity
    {
        DataExpressions<T> Build();
    }
}
