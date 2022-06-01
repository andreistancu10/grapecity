using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetUnseenCountByUserId
{
    public sealed class GetUnseenCountByUserIdQuery : AbstractFilterModel, IQuery<GetUnseenCountByUserIdResponse>
    {
    }
}