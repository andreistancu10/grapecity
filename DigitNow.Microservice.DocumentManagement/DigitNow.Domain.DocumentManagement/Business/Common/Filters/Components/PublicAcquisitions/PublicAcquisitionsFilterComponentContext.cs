using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.PublicAcquisitions
{
    internal class PublicAcquisitionsFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public PublicAcquisitionFilter PublicAcquisitionFilter { get; set; }
    }
}
