using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using MediatR.Pipeline;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Services;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Update
{
    public class UpdateNotificationPostProcessor : IRequestPostProcessor<UpdateNotificationCommand, ResultObject>
    {
        private readonly INotificationService _notificationService;

        public UpdateNotificationPostProcessor(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }

        public async Task Process(UpdateNotificationCommand request,
            ResultObject resultObject,
            CancellationToken cancellationToken)
        {
            if (resultObject.StatusCode.Equals(ResultStatusCode.Ok))
                await _notificationService.BuildAndSendNotificationToSyncAsync(request.Id, cancellationToken);
        }
    }
}