using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetUnseenCountByUserId
{
    public sealed class GetUnseenCountByUserIdQuery : AbstractFilterModel, IQuery<GetUnseenCountByUserIdResponse>
    {
    }
}