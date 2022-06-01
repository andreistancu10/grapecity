using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;

namespace DigitNow.Domain.DocumentManagement.Data.NotificationStatuses.Seed
{
    public static class Data
    {
        public static IEnumerable<NotificationStatus> GetNotificationStatuses()
        {
            return new[]
            {
                new NotificationStatus((long) NotificationStatusEnum.Pending)
                {
                    Name = NotificationStatusEnum.Pending.ToString(), Code = NotificationStatusEnum.Pending.ToString(),
                    Active = true
                },
                new NotificationStatus((long) NotificationStatusEnum.Approved)
                {
                    Name = NotificationStatusEnum.Approved.ToString(),
                    Code = NotificationStatusEnum.Approved.ToString(), Active = true
                },
                new NotificationStatus((long) NotificationStatusEnum.Rejected)
                {
                    Name = NotificationStatusEnum.Rejected.ToString(),
                    Code = NotificationStatusEnum.Rejected.ToString(), Active = true
                },
                new NotificationStatus((long) NotificationStatusEnum.Cancelled)
                {
                    Name = NotificationStatusEnum.Cancelled.ToString(),
                    Code = NotificationStatusEnum.Cancelled.ToString(), Active = true
                }
            };
        }
    }
}