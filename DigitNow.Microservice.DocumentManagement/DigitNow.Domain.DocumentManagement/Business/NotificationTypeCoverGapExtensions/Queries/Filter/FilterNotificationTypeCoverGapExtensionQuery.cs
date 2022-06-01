using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.Filter
{
    public sealed class FilterNotificationTypeCoverGapExtensionQuery : AbstractFilterModel, IQuery<ResultPagedList<FilterNotificationTypeCoverGapExtensionResponse>>
    {
        public long? Id { get; set; }

        public long? NotificationTypeId { get; set; }

        public bool? Active { get; set; }
    }
}
