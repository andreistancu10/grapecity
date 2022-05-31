using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetListByStatus
{
    public sealed class GetNotificationTypesByNotificationStatusIdQuery : IQuery<IList<GetNotificationTypesByNotificationStatusIdResponse>>
    {
        public long NotificationStatusId { get; set; }
    }
}