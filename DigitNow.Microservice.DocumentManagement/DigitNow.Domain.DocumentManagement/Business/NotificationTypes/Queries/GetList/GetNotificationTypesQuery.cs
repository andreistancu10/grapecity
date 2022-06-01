using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetList
{
    public sealed class GetNotificationTypesQuery : IQuery<IList<GetNotificationTypesResponse>>
    {
    }
}