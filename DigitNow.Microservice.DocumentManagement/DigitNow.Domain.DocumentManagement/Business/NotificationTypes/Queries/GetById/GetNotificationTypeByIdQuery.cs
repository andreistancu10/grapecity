using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetById
{
    public sealed class GetNotificationTypeByIdQuery : IQuery<GetNotificationTypeByIdResponse>
    {
        public long Id { get; init; }
    }
}