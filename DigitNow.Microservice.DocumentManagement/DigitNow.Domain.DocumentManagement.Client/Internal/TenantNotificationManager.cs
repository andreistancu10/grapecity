using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Client.Internal.Model;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationsStatusForEntity;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationStatus;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.CreateNotification;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.GetList;
using HTSS.Platform.Infrastructure.MassTransit;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Client.Internal
{
    internal class TenantNotificationManager : ITenantNotificationManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMassTransitRpcClient _rpcClient;

        public TenantNotificationManager(IMassTransitRpcClient rpcClient, IServiceProvider serviceProvider)
        {
            _rpcClient = rpcClient;
            _serviceProvider = serviceProvider;
        }

        public async Task<IGetNotificationByIdResponse> GetNotificationById(long id, CancellationToken cancellationToken)
        {
            var mqRequest = new GetNotificationByIdRequest
            {
                Id = id
            };

            var mqResponse = await _rpcClient.Execute<IGetNotificationByIdRequest, RpcResponse<IGetNotificationByIdResponse>>(mqRequest, cancellationToken);
            return mqResponse.Body;
        }

        public Task CreateNotification(ICreateNotificationEvent createNotificationEvent, CancellationToken cancellationToken)
        {
            return ExecutePublish(createNotificationEvent, cancellationToken);
        }

        public Task ChangeNotificationStatus(long notificationId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken)
        {
            var mqRequest = new ChangeNotificationStatusEvent
            {
                NotificationId = notificationId,
                NotificationStatus = notificationStatus
            };

            return ExecutePublish<IChangeNotificationStatusEvent>(mqRequest, cancellationToken);
        }

        public Task ChangeNotificationsStatusForEntity(long entityId, NotificationEntityTypeEnum entityTypeId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken)
        {
            var mqRequest = new ChangeNotificationsStatusForEntityEvent
            {
                EntityId = entityId,
                EntityTypeId = entityTypeId,
                NotificationStatus = notificationStatus
            };

            return ExecutePublish<IChangeNotificationsStatusForEntityEvent>(mqRequest, cancellationToken);
        }

        public async Task<IGetNotificationTypeCoverGapExtensionsResponse> GetNotificationTypeCoverGapExtensions(string notificationTypeStatus, CancellationToken cancellationToken)
        {
            var mqRequest = new GetNotificationTypeCoverGapExtensionsRequest
            {
                NotificationTypeStatus = notificationTypeStatus
            };

            var mqResponse = await _rpcClient.Execute<IGetNotificationTypeCoverGapExtensionsRequest, RpcResponse<IGetNotificationTypeCoverGapExtensionsResponse>>(mqRequest, cancellationToken);
            return mqResponse.Body;
        }

        private Task ExecutePublish<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : class
        {
            return _serviceProvider.GetRequiredService<IPublishEndpoint>().Publish(request, cancellationToken);
        }
    }
}