﻿using System;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using MassTransit;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Update
{
    public class UpdateNotificationTypeCoverGapExtensionPostProcessor : IRequestPostProcessor<UpdateNotificationTypeCoverGapExtensionCommand, ResultObject>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateNotificationTypeCoverGapExtensionPostProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public Task Process(UpdateNotificationTypeCoverGapExtensionCommand request, ResultObject response, CancellationToken cancellationToken)
        {
            return _serviceProvider.GetRequiredService<IPublishEndpoint>().Publish<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand>(new NotificationTypeCoverGapExtensionElasticUpdateIndexCommand
            {
                SynchronizationStrategy = INotificationTypeCoverGapExtensionElasticUpdateIndexCommand.SynchronizationStrategyCode.All
            }, cancellationToken);
        }
    }
}