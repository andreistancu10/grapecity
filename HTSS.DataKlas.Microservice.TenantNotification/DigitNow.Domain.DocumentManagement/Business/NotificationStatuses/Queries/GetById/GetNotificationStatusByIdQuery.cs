using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetById
{
    public sealed class GetNotificationStatusByIdQuery : IQuery<GetNotificationStatusByIdResponse>
    {
        public long Id { get; init; }
    }
}