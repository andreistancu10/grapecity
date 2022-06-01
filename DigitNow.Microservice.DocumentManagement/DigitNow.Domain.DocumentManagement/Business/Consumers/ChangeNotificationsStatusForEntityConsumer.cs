using System;
using System.Linq;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationsStatusForEntity;
using DigitNow.Domain.DocumentManagement.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.Consumers
{
    public sealed class ChangeNotificationsStatusForEntityConsumer : IConsumer<IChangeNotificationsStatusForEntityEvent>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILogger<ChangeNotificationsStatusForEntityConsumer> _logger;
        private readonly INotificationService _notificationService;

        public ChangeNotificationsStatusForEntityConsumer(
            ILogger<ChangeNotificationsStatusForEntityConsumer> logger,
            DocumentManagementDbContext dbContext,
            INotificationService notificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<IChangeNotificationsStatusForEntityEvent> context)
        {
            _logger.LogInformation("{IChangeNotificationsStatusForEntityEvent} event Received", nameof(IChangeNotificationsStatusForEntityEvent));

            var responseNotificationStatus = await _dbContext.NotificationStatuses.AsNoTracking()
                .Where(x => x.Id == (long) context.Message.NotificationStatus)
                .FirstOrDefaultAsync(context.CancellationToken);

            var notificationsToUpdate = await _dbContext.Notifications
                .Where(x => x.EntityId == context.Message.EntityId &&
                            x.EntityTypeId == (int) context.Message.EntityTypeId)
                .ToListAsync(context.CancellationToken);

            foreach (var notification in notificationsToUpdate)
            {
                notification.NotificationStatusId = responseNotificationStatus.Id;
                notification.ModifiedOn = DateTime.Now;
                notification.Seen = true;
                notification.SeenOn = DateTime.Now;
            }

            await _dbContext.BulkUpdateAsync(notificationsToUpdate, context.CancellationToken);

            foreach (var notification in notificationsToUpdate)
                await _notificationService.BuildAndSendNotificationToSyncAsync(notification.Id,
                    context.CancellationToken);
        }
    }
}