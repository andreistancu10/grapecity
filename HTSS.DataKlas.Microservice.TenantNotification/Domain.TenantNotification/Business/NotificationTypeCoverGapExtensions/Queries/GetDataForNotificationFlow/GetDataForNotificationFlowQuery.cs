using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.GetDataForNotificationFlow
{
    public sealed class GetDataForNotificationFlowQuery : AbstractFilterModel, IQuery<GetDataForNotificationFlowResponse>
    {
    }
}
