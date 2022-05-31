using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions;
using HTSS.Platform.Core.CQRS;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Sync
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
