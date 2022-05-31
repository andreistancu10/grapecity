using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetById
{
    public sealed class GetNotificationByIdQuery : IQuery<GetNotificationByIdResponse>
    {
        public long Id { get; init; }
    }
}