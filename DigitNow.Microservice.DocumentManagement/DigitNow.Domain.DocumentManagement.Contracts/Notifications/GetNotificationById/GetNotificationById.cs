using System;

namespace DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById
{
    public interface IGetNotificationByIdRequest
    {
        long Id { get; }
    }

    public interface IGetNotificationByIdResponse
    {
        long Id { get; }

        string Message { get; }

        long NotificationTypeId { get; }

        string NotificationTypeName { get; }

        string NotificationStatusName { get; }

        long NotificationStatusId { get; }

        long UserId { get; }

        string UserName { get; }

        long? FromUserId { get; }

        string FromUserName { get; }

        long? EntityId { get; }

        long? EntityTypeId { get; }

        string EntityTypeName { get; }

        bool Seen { get; }

        bool IsInformative { get; }

        bool IsUrgent { get; }

        DateTime? SeenOn { get; }

        DateTime CreatedOn { get; }

        DateTime? ModifiedOn { get; }
    }
}