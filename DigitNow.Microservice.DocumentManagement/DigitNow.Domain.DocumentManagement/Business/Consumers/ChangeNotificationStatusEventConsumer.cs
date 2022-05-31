using System;
using System.Linq;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationStatus;
using DigitNow.Domain.DocumentManagement.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.Consumers
{
    public sealed class ChangeNotificationStatusEventConsumer : IConsumer<IChangeNotificationStatusEvent>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILogger<ChangeNotificationStatusEventConsumer> _logger;
        private readonly INotificationService _notificationService;

        public ChangeNotificationStatusEventConsumer(ILogger<ChangeNotificationStatusEventConsumer> logger,
            DocumentManagementDbContext dbContext,
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