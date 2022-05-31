using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetCategories
{
    public sealed class GetCategoriesQuery : IQuery<IList<GetCategoriesResponse>>
    {
    }
}
