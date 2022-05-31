using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.GetList
{
    public sealed class GetNotificationStatusesQuery : IQuery<IList<GetNotificationStatusesResponse>>
    {
    }
}