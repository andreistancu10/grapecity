using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using MediatR.Pipeline;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Services;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.ChangeStatus
{
    public class
        ChangeNotificationStatusPostProcessor : IRequestPostProcessor<ChangeNotificationStatusCommand, ResultObject>
    {
        private readonly INotificationService _notificationService;

        public ChangeNotificationStatusPostProcessor(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }

        public async Task Process(ChangeNotificationStatusCommand request,
            ResultObject resultObject,
            CancellationToken cancellationToken)
        {
            if (resultObject.StatusCode.Equals(ResultStatusCode.Ok))
                await _notificationService.BuildAndSendNotificationToSyncAsync(request.Id, cancellationToken);
        }
    }
}