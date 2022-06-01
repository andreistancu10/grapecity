using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Consumers;
using HTSS.Platform.Core.CQRS;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Sync
{
    internal sealed class SyncNotificationsHandler : ICommandHandler<SyncNotificationsCommand, ResultObject>
    {
        private readonly ILogger<SyncNotificationsHandler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SyncNotificationsHandler(ILogger<SyncNotificationsHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<ResultObject> Handle(SyncNotificationsCommand command, CancellationToken cancellationToken)
        {
            await _serviceProvider.GetRequiredService<IPublishEndpoint>().Publish<INotificationElasticUpdateIndexCommand>(
                new NotificationElasticUpdateIndexCommand
                    {SynchronizationStrategy = INotificationElasticUpdateIndexCommand.SynchronizationStrategyCode.All},
                cancellationToken);

            _logger.LogInformation("Sync notifications evt Sent");

            return ResultObject.Ok();
        }
    }
}