using System;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Infrastructure.MassTransit;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.TenantNotification.Client.Internal.Model;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationsStatusForEntity;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationStatus;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.CreateNotification;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.GetNotificationById;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace ShiftIn.Domain.TenantNotification.Client.Internal
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