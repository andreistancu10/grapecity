using System.Threading;
using System.Threading.Tasks;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.CreateNotification;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.GetNotificationById;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace ShiftIn.Domain.TenantNotification.Client
{
    public interface ITenantNotificationManager
    {
        Task<IGetNotificationByIdResponse> GetNotificationById(long id, CancellationToken cancellationToken);
        
        Task CreateNotification(ICreateNotificationEvent createNotificationEvent, CancellationToken cancellationToken);
        
        Task ChangeNotificationStatus(long notificationId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken);
        
        Task ChangeNotificationsStatusForEntity(long entityId, NotificationEntityTypeEnum entityTypeId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken);

        Task<IGetNotificationTypeCoverGapExtensionsResponse> GetNotificationTypeCoverGapExtensions(string notificationTypeStatus, CancellationToken cancellationToken);
    }
}