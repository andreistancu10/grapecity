using System;
using HTSS.Platform.Core.CQRS;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Sync
{
    internal sealed class SyncNotificationTypeCoverGapExtensionHandler : ICommandHandler<SyncNotificationTypeCoverGapExtensionCommand, ResultObject>
    {
        private readonly ILogger<SyncNotificationTypeCoverGapExtensionHandler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SyncNotificationTypeCoverGapExtensionHandler(ILogger<SyncNotificationTypeCoverGapExtensionHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<ResultObject> Handle(SyncNotificationTypeCoverGapExtensionCommand request, CancellationToken cancellationToken)
        {
            await _serviceProvider.GetRequiredService<IPublishEndpoint>().Publish<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand>(new NotificationTypeCoverGapExtensionElasticUpdateIndexCommand() { SynchronizationStrategy = INotificationTypeCoverGapExtensionElasticUpdateIndexCommand.SynchronizationStrategyCode.All }, cancellationToken);

            _logger.LogInformation("Sync NotificationTypeCoverGapExtension evt Sent");

            return ResultObject.Ok();
        }
    }
}
