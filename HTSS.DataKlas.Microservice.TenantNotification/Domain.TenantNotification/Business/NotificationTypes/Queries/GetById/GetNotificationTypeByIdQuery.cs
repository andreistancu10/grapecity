using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetById
{
    public sealed class GetNotificationTypeByIdQuery : IQuery<GetNotificationTypeByIdResponse>
    {
        public long Id { get; init; }
    }
}