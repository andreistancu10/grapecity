using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetList
{
    public sealed class GetNotificationsQuery : IQuery<IList<GetNotificationsResponse>>
    {
    }
}