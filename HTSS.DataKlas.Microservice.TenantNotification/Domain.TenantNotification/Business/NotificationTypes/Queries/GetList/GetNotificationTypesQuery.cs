using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetList
{
    public sealed class GetNotificationTypesQuery : IQuery<IList<GetNotificationTypesResponse>>
    {
    }
}