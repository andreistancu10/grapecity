﻿using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Create
{
    public sealed class CreateNotificationCommand : ICommand<ResultObject>
    {
        public long NotificationTypeId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }
    }
}