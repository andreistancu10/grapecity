using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetListByStatus
{
    public sealed class GetNotificationTypesByNotificationStatusIdQuery : IQuery<IList<GetNotificationTypesByNotificationStatusIdResponse>>
    {
        public long NotificationStatusId { get; set; }
    }
}