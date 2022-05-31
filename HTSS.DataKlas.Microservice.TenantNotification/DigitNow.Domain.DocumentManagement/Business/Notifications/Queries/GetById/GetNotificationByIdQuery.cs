using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetById
{
    public sealed class GetNotificationByIdQuery : IQuery<GetNotificationByIdResponse>
    {
        public long Id { get; init; }
    }
}