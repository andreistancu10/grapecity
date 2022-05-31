using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetList
{
    public sealed class GetNotificationsQuery : IQuery<IList<GetNotificationsResponse>>
    {
    }
}