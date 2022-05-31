using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Services;
using HTSS.Platform.Core.CQRS;
using MediatR.Pipeline;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.RejectNotification
{
    public class RejectNotificationPostProcessor : IRequestPostProcessor<RejectNotificationCommand, ResultObject>
    {
        private readonly INotificationService _notificationService;

        public RejectNotificationPostProcessor(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }

        public async Task Process(RejectNotificationCommand request,
            ResultObject resultObject,
            CancellationToken cancellationToken)
        {
            if (resultObject.StatusCode.Equals(ResultStatusCode.Ok))
                await _notificationService.BuildAndSendNotificationToSyncAsync(request.Id, cancellationToken);
        }
    }
}