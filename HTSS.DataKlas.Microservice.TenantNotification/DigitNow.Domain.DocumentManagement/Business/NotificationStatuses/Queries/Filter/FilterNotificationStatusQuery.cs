using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.Filter
{
    public sealed class FilterNotificationStatusQuery : AbstractFilterModel<FilterNotificationStatusQuery>, IQuery<ResultPagedList<FilterNotificationStatusResponse>>
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool? Active { get; set; }
    }
}