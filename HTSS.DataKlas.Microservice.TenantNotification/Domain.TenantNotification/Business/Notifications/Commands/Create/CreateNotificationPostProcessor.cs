using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using MediatR.Pipeline;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Services;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Create
{
    public class CreateNotificationPostProcessor : IRequestPostProcessor<CreateNotificationCommand, ResultObject>
    {
        private readonly INotificationService _notificationService;

        public CreateNotificationPostProcessor(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }

        public async Task Process(CreateNotificationCommand request,
            ResultObject resultObject,
            CancellationToken cancellationToken)
        {
            if (resultObject.StatusCode.Equals(ResultStatusCode.Created))
                await _notificationService.BuildAndSendNotificationToSyncAsync((long) resultObject.Data,
                    cancellationToken);
        }
    }
}