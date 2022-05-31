using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetEntityTypes
{
    public sealed class GetEntityTypesQuery : IQuery<IList<GetEntityTypesResponse>>
    {
    }
}
