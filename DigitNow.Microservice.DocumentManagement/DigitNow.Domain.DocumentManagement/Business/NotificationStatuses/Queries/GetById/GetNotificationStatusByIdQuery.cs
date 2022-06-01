using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.GetById
{
    public sealed class GetNotificationStatusByIdQuery : IQuery<GetNotificationStatusByIdResponse>
    {
        public long Id { get; init; }
    }
}