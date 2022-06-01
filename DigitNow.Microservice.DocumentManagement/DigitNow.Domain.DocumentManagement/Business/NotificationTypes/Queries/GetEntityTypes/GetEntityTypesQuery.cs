using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetEntityTypes
{
    public sealed class GetEntityTypesQuery : IQuery<IList<GetEntityTypesResponse>>
    {
    }
}
