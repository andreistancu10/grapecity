using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Services;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationStatus;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.Consumers
{
    public sealed class ChangeNotificationStatusEventConsumer : IConsumer<IChangeNotificationStatusEvent>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly ILogger<ChangeNotificationStatusEventConsumer> _logger;
        private readonly INotificationService _notificationService;

        public ChangeNotificationStatusEventConsumer(ILogger<ChangeNotificationStatusEventConsumer> logger,
            TenantNotificationDbContext dbContext,
            INotificationService notificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<IChangeNotificationStatusEvent> context)
        {
            _logger.LogInformation("{IChangeNotificationStatusEvent} event Received", nameof(IChangeNotificationStatusEvent));

            var entity =
                await _dbContext.Notifications.FindAsync(context.Message.NotificationId, context.CancellationToken);

            if (entity != null)
            {
                var responseNotificationStatus = await _dbContext.NotificationStatuses.AsNoTracking()
                    .Where(x => x.Id == (long) context.Message.NotificationStatus)
                    .FirstOrDefaultAsync(context.CancellationToken);

                if (responseNotificationStatus != null)
                {
                    entity.NotificationStatusId = responseNotificationStatus.Id;
                    entity.ModifiedOn = DateTime.Now;
                    entity.Seen = true;
                    entity.SeenOn = DateTime.Now;

                    await _dbContext.SaveChangesAsync(context.CancellationToken);

                    await _notificationService.BuildAndSendNotificationToSyncAsync(context.Message.NotificationId,
                        context.CancellationToken);
                }
            }
        }
    }
}