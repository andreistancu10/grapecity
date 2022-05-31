using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetCategories
{
    public sealed class GetCategoriesQuery : IQuery<IList<GetCategoriesResponse>>
    {
    }
}
