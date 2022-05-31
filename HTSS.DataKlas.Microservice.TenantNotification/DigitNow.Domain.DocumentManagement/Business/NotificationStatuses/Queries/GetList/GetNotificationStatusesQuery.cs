using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetList
{
    public sealed class GetNotificationStatusesQuery : IQuery<IList<GetNotificationStatusesResponse>>
    {
    }
}